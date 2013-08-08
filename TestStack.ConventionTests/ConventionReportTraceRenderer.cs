namespace TestStack.ConventionTests
{
    using System.Diagnostics;

    public class ConventionReportTraceRenderer : IConventionReportRenderer
    {
        public void Render(params ConventionReport[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Render(conventionResult);
            Trace.WriteLine(conventionReportTextRenderer.Output);
        }
    }
}