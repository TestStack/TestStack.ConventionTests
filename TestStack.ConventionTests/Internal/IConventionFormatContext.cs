namespace TestStack.ConventionTests.Internal
{
    using TestStack.ConventionTests.Reporting;

    public interface IConventionFormatContext
    {
        ConventionReportFailure FormatData(object failingData);
        ITestResultProcessor TestResultProcessor { get; }
    }
}