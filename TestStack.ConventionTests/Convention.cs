namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Reporting;

    public static class Convention
    {
        static readonly HtmlReportRenderer HtmlRenderer = new HtmlReportRenderer(AssemblyDirectory);
        static readonly List<ConventionResult> Reports = new List<ConventionResult>();

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

        public static IEnumerable<ConventionResult> ConventionReports { get { return Reports; } }
        public static IList<IReportDataFormatter> Formatters { get; set; } 

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new ThrowOnFailureResultsProcessor());
        }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IResultsProcessor processor)
            where TDataSource : IConventionData
        {
            try
            {
                var context = new ConventionContext(data.Description, Formatters);
                var conventionResult = context.Execute(convention, data);
                Reports.AddRange(conventionResult);

                new ConventionReportTraceRenderer().Process(conventionResult);
                processor.Process(conventionResult);
            }
            finally
            {
                HtmlRenderer.Process(Reports.ToArray());
            }
        }

        public static void IsWithApprovedExeptions<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new ApproveResultsProcessor());
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