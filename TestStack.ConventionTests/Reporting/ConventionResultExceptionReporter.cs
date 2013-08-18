namespace TestStack.ConventionTests.Reporting
{
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class ConventionResultExceptionReporter : IResultsProcessor
    {
        public void Process(params ConventionResult[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Process(conventionResult);
            if (conventionResult.Any(r => r.Result == TestResult.Failed))
            {
                throw new ConventionFailedException(conventionReportTextRenderer.Output);
            }
        }
    }
}