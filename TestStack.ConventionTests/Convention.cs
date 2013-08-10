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

        public static IEnumerable<ResultInfo> ConventionReports { get { return Reports; } }
        public static IList<IReportDataFormatter> Formatters { get; private set; } 

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new ConventionResultExceptionReporter());
        }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data, IConventionReportRenderer reporter) 
            where TDataSource : IConventionData
        {
            try
            {
                var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);

                Reports.Add(conventionResult);

                new ConventionReportTraceRenderer().Render(conventionResult);
                reporter.Render(conventionResult);
            }
            finally
            {
                HtmlRenderer.Render(Reports.ToArray());
            }
        }

        public static void IsWithApprovedExeptions<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);
            Reports.Add(conventionResult);

            try
            {
                var conventionReportTextRenderer = new ConventionReportTextRenderer();
                conventionReportTextRenderer.RenderItems(conventionResult);
                conventionResult.WithApprovedException(conventionReportTextRenderer.Output);

                conventionReportTextRenderer.Render(conventionResult);
                Approvals.Verify(conventionReportTextRenderer.Output);

                new ConventionReportTraceRenderer().Render(conventionResult);
            }
            catch (ApprovalException ex)
            {
                throw new ConventionFailedException("Approved exceptions for convention differs\r\n\r\n"+ex.Message, ex);
            }
            finally
            {
                HtmlRenderer.Render(Reports.ToArray());
            }
        }

        public static void Is<TDataSource>(ISymmetricConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new ConventionResultExceptionReporter());
        }

        public static void Is<TDataSource>(ISymmetricConvention<TDataSource> convention, TDataSource data, IConventionReportRenderer reporter)
            where TDataSource : IConventionData
        {
            try
            {
                var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);
                var inverseConventionResult = GetConventionReport(convention.InverseTitle, convention.GetFailingInverseData(data).ToArray(), data);

                Reports.Add(conventionResult);
                Reports.Add(inverseConventionResult);

                new ConventionReportTraceRenderer().Render(conventionResult, inverseConventionResult);
                reporter.Render(conventionResult, inverseConventionResult);
            }
            finally
            {
                HtmlRenderer.Render(Reports.ToArray());
            }
        }

        public static void IsWithApprovedExeptions<TDataSource>(ISymmetricConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);
            var inverseConventionResult = GetConventionReport(convention.InverseTitle, convention.GetFailingInverseData(data).ToArray(), data);
            Reports.Add(conventionResult);
            Reports.Add(inverseConventionResult);

            try
            {
                var conventionReportTextRenderer = new ConventionReportTextRenderer();
                // Add approved exceptions to report
                conventionReportTextRenderer.RenderItems(conventionResult);
                conventionResult.WithApprovedException(conventionReportTextRenderer.Output);

                // Add approved exceptions to inverse report
                conventionReportTextRenderer.RenderItems(inverseConventionResult);
                inverseConventionResult.WithApprovedException(conventionReportTextRenderer.Output);

                //Render both, with approved exceptions included
                conventionReportTextRenderer.Render(conventionResult, inverseConventionResult);
                Approvals.Verify(conventionReportTextRenderer.Output);
                
                // Trace on success
                new ConventionReportTraceRenderer().Render(conventionResult, inverseConventionResult);
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

        static ResultInfo GetConventionReport<TDataSource, TDataType>(string conventionTitle, TDataType[] failingData, TDataSource data)
            where TDataSource : IConventionData
        {
            data.EnsureHasNonEmptySource();
            var passed = failingData.None();

            var conventionResult = new ResultInfo(
                passed ? TestResult.Passed : TestResult.Failed,
                conventionTitle,
                data.Description,
                failingData.Select(FormatData).ToArray());
            return conventionResult;
        }

        static ConventionReportFailure FormatData<T>(T failingData)
        {
            var formatter = Formatters.FirstOrDefault(f => f.CanFormat(failingData));

            if (formatter == null)
                throw new NoDataFormatterFoundException(typeof(T).Name + " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");

            return formatter.Format(failingData);
        }

        // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
        static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}