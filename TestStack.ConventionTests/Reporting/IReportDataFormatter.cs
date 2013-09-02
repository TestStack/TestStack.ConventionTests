namespace TestStack.ConventionTests.Reporting
{
    public interface IReportDataFormatter
    {
        bool CanFormat(object data);
        string FormatString(object data);
        string FormatHtml(object data);
    }
}