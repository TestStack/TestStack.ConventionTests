namespace TestStack.ConventionTests.Reporting
{
    public class FallbackFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return true;
        }

        public string FormatString(object failingData)
        {
            if (failingData == null)
                return "<null>";
            return failingData.ToString();
        }
    }
}