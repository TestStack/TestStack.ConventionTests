namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;

    public class ProjectFileFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is ProjectFileItem;
        }

        public string FormatString(object failingData)
        {
            return ((ProjectFileItem)failingData).FilePath;
        }
    }
}