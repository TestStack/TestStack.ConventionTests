namespace TestStack.ConventionTests.Internal
{
    public interface IConventionFormatContext
    {
        ConventionReportFailure FormatData(object failingData);
    }
}