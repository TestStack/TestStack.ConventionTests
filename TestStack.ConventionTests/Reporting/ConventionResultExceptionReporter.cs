namespace TestStack.ConventionTests.Reporting
{
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class ConventionResultExceptionReporter : IConventionReportRenderer
    {
        public void Render(params ResultInfo[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Render(conventionResult);
            if (conventionResult.Any(r => r.Result == TestResult.Failed))
            {
                throw new ConventionFailedException(conventionReportTextRenderer.Output);
            }
        }
    }
}