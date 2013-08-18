namespace TestStack.ConventionTests.Reporting
{
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class ThrowOnFailureResultsProcessor : IResultsProcessor
    {
        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Process(context, results);
            if (results.Any(r => r.HasData))
            {
                throw new ConventionFailedException(conventionReportTextRenderer.Output);
            }
        }
    }
}