namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using TestStack.ConventionTests.Internal;

    /// <summary>
    /// Aggregates all previous results
    /// </summary>
    public abstract class AggregatedConventionResultsReporter : IResultsProcessor
    {
        static readonly List<ConventionResult> Reports = new List<ConventionResult>();
        readonly string output;

        protected AggregatedConventionResultsReporter(string outputFilename)
        {
            output = Path.Combine(AssemblyDirectory, outputFilename);
        }

        static string AssemblyDirectory
        {
            get
            {
                // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917

                var codeBase = Assembly.GetEntryAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public IEnumerable<ConventionResult> AggregatedReports { get { return Reports; } }

        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            Reports.AddRange(results.Except(Reports));
            var outputContent = Process(context);
            File.WriteAllText(output, outputContent);
        }

        protected abstract string Process(IConventionFormatContext context);
    }
}