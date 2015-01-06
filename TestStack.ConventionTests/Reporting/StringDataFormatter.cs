namespace TestStack.ConventionTests.Reporting
{
    public class StringDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return data is string;
        }

        public string FormatString(object data)
        {
            return (string)data;
        }

        public string FormatHtml(object data)
        {
            return (string)data;
        }
    }
}