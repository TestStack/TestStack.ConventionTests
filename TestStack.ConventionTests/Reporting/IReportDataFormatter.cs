namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public interface IReportDataFormatter
    {
        bool CanFormat(object failingData);
        ConventionReportFailure Format(object failingData);
    }
}