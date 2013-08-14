namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using ApprovalTests;
    using ApprovalTests.Core.Exceptions;
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
        public static IList<IReportDataFormatter> Formatters { get; set; } 

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
                var conventionResult = Executor.GetConventionResults(convention, data);
                Reports.AddRange(conventionResult);

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
            var conventionResult = Executor.GetConventionResultsWithApprovedExeptions(convention, data);
            Reports.AddRange(conventionResult);

            try
            {
                var conventionReportTextRenderer = new ConventionReportTextRenderer();
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