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

        public static IList<IReportDataFormatter> Formatters { get; set; }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new IResultsProcessor[]
            {
                HtmlRenderer,
                new ConventionReportTraceRenderer(),
                new ThrowOnFailureResultsProcessor()
            });
        }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IResultsProcessor[] processors)
            where TDataSource : IConventionData
        {
            var context = new ConventionContext(data.Description, Formatters);
            var conventionResult = context.Execute(convention, data);

            foreach (var processor in processors)
            {
                processor.Process(conventionResult);
            }
        }

        public static void IsWithApprovedExeptions<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new IResultsProcessor[]
            {
                HtmlRenderer,
                new ConventionReportTraceRenderer(),
                new ApproveResultsProcessor()
            });
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