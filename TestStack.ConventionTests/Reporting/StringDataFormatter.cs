namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public class StringDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is string;
        }

        public ConventionReportFailure Format(object failingData)
        {
            return new ConventionReportFailure((string)failingData);
        }
    }
}