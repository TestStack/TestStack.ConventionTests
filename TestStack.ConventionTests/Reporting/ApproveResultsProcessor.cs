namespace TestStack.ConventionTests.Reporting
{
    using ApprovalTests;
    using ApprovalTests.Core.Exceptions;
    using TestStack.ConventionTests.Internal;

    public class ApproveResultsProcessor : IResultsProcessor
    {
        public void Process(params ConventionResult[] results)
        {
            try
            {
                var conventionReportTextRenderer = new ConventionReportTextRenderer();
                conventionReportTextRenderer.Process(results);
                Approvals.Verify(conventionReportTextRenderer.Output);
            }
            catch (ApprovalException ex)
            {
                throw new ConventionFailedException("Approved exceptions for convention differs\r\n\r\n" + ex.Message,
                    ex);
            }
        }
    }
}