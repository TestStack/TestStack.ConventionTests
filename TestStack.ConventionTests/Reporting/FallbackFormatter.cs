namespace TestStack.ConventionTests.Reporting
{
    public class FallbackFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return true;
        }

        public string FormatString(object data)
        {
            if (data == null)
                return "<null>";
            return data.ToString();
        }

        public string FormatHtml(object data)
        {
            return FormatString(data);
        }
    }
}