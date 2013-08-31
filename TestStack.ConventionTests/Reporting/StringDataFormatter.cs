namespace TestStack.ConventionTests.Reporting
{
    public class StringDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is string;
        }

        public string FormatString(object failingData)
        {
            return (string)failingData;
        }
    }
}