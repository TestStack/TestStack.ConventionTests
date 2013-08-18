namespace TestStack.ConventionTests.Reporting
{
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class ThrowOnFailureResultsProcessor : IResultsProcessor
    {
        public void Process(params ConventionResult[] results)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Process(results);
            if (results.Any(r => r.Result == TestResult.Failed))
            {
                throw new ConventionFailedException(conventionReportTextRenderer.Output);
            }
        }
    }
}