namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
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
                new StringDataFormatter(),
                new ConvertibleFormatter(),
                new FallbackFormatter()
            };
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

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            params IResultsProcessor[] extraResultProcessors)
            where TDataSource : IConventionData
        {
            var processors = new List<IResultsProcessor>(extraResultProcessors)
            {
                new ConventionReportTextRenderer(),
                HtmlRenderer,
                new ConventionReportTraceRenderer(),
                new ThrowOnFailureResultsProcessor()
            };
            Execute(convention, data, processors.ToArray());
        }

        public static void IsWithApprovedExeptions<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            params IResultsProcessor[] extraResultProcessors)
            where TDataSource : IConventionData
        {
            var processors = new List<IResultsProcessor>(extraResultProcessors)
            {
                new ConventionReportTextRenderer(),
                HtmlRenderer,
                new ConventionReportTraceRenderer(),
                new ApproveResultsProcessor()
            };
            Execute(convention, data, processors.ToArray());
        }

        static void Execute<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IResultsProcessor[] processors)
            where TDataSource : IConventionData
        {
            var context = new ConventionContext(string.Format("{0} in {1}", ToSentenceCase(data.GetType().Name), data.Description), Formatters, processors);
            context.Execute(convention, data);
        }

        static string ToSentenceCase(string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
    }
}