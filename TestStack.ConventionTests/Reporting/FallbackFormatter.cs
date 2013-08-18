namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public class FallbackFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return true;
        }

        public ConventionReportFailure Format(object failingData)
        {
            // TODO: for now
            return new ConventionReportFailure(failingData.ToString());
        }
    }
}