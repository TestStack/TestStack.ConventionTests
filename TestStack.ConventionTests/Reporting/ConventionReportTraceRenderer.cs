namespace TestStack.ConventionTests.Reporting
{
    using System.Diagnostics;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTraceRenderer : IResultsProcessor
    {
        public void Process(params ConventionResult[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Process(conventionResult);
            Trace.WriteLine(conventionReportTextRenderer.Output);
        }
    }
}