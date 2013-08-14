namespace TestStack.ConventionTests.Reporting
{
    using System.Diagnostics;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTraceRenderer : IConventionReportRenderer
    {
        public void Render(params ConventionResult[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Render(conventionResult);
            Trace.WriteLine(conventionReportTextRenderer.Output);
        }
    }
}