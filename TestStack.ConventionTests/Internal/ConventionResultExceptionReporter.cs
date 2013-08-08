namespace TestStack.ConventionTests.Internal
{
    using System.Linq;

    public class ConventionResultExceptionReporter : IConventionReportRenderer
    {
        public void Render(params ConventionReport[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Render(conventionResult);
            if (conventionResult.Any(r => r.Result == Result.Failed))
            {
                throw new ConventionFailedException(conventionReportTextRenderer.Output);
            }
        }
    }
}