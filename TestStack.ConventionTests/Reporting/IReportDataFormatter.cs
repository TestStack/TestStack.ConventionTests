namespace TestStack.ConventionTests.Reporting
{
    public interface IReportDataFormatter
    {
        bool CanFormat(object failingData);
        string FormatString(object failingData);
    }
}