namespace TestStack.ConventionTests.Reporting
{
    using System.Diagnostics;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTraceRenderer : IResultsProcessor
    {
        public void Process(params ConventionResult[] results)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Process(results);
            Trace.WriteLine(conventionReportTextRenderer.Output);
        }
    }
}