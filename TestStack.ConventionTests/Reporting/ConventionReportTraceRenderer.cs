namespace TestStack.ConventionTests.Reporting
{
    using System.Diagnostics;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTraceRenderer : IResultsProcessor
    {
        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            foreach (var conventionResult in results)
            {
                Trace.WriteLine(conventionResult.FormattedResult);
            }
        }
    }
}