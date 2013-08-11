namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ApprovalTests;
    using ApprovalTests.Core.Exceptions;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Reporting;

    public static class Convention
    {
        static readonly HtmlReportRenderer HtmlRenderer = new HtmlReportRenderer(AssemblyDirectory);
        static readonly List<ResultInfo> Reports = new List<ResultInfo>();

        static Convention()
        {
            Formatters = new List<IReportDataFormatter>
            {
                new TypeDataFormatter(),
                new ProjectReferenceFormatter(),
                new ProjectFileFormatter(),
                new MethodInfoDataFormatter(),
                new StringDataFormatter()
            };
        }

        public static IEnumerable<ResultInfo> ConventionReports
        {
            get { return Reports; }
        }

        public static IList<IReportDataFormatter> Formatters { get; set; }

        static string AssemblyDirectory
        {
            get
            {
                // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new ConventionResultExceptionReporter());
        }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IConventionReportRenderer reporter)
            where TDataSource : IConventionData
        {
            try
            {
                // we want to run that first so that we don't even bother running the convention if theere's no data
                // conveniton author can assume that data is available. That will simplify the conventions
                if (!data.HasData)
                    throw new ConventionSourceInvalidException(string.Format("{0} has no data", data.Description));

                var result = new ConventionContext(data, Formatters, reporter, new ConventionReportTraceRenderer());

                convention.Execute(data, result);
                Render(convention, data, result, Executor.GetConventionReport);
            }
            finally
            {
                HtmlRenderer.Render(Reports.ToArray());
            }
        }

        //ResultInfo GetConventionReport(string conventionTitle, IConventionData data, IEnumerable<object> items, bool failed)
        static void Render<TDataSource>(IConvention<TDataSource> convention, TDataSource data, ConventionContext result,
            Func<string, IConventionContext, IEnumerable<object>, ResultInfo> getConventionReport) where TDataSource : IConventionData
        {
            if (result.IsSymmetricResult)
            {
                var result1 = getConventionReport(result.FirstDescription,
                    result, result.FirstOnly);
                var result2 = getConventionReport(result.SecondDescription,
                    result, result.SecondOnly);
                Reports.Add(result1);
                Reports.Add(result2);

                foreach (var renderer in result.Renderers)
                {
                    renderer.Render(result1, result2);
                }
            }
            else
            {
                var conventionResult = getConventionReport(convention.ConventionTitle, result, result.Items);
                Reports.Add(conventionResult);

                foreach (var renderer in result.Renderers)
                {
                    renderer.Render(conventionResult);
                }
            }
        }

        public static void IsWithApprovedExeptions<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            // we want to run that first so that we don't even bother running the convention if theere's no data
            // conveniton author can assume that data is available. That will simplify the conventions
            if (!data.HasData)
                throw new ConventionSourceInvalidException(string.Format("{0} has no data", data.Description));

            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            var result = new ConventionContext(data, Formatters, conventionReportTextRenderer, new ConventionReportTraceRenderer());
            convention.Execute(data, result);

            Render(convention, data, result, Executor.GetConventionReportWithApprovedExeptions);
            try
            {
                Approvals.Verify(conventionReportTextRenderer.Output);
            }
            catch (ApprovalException ex)
            {
                throw new ConventionFailedException("Approved exceptions for convention differs\r\n\r\n" + ex.Message, ex);
            }
            finally
            {
                HtmlRenderer.Render(Reports.ToArray());
            }
        }
    }
}