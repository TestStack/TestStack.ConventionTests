namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;

    public class ProjectFileFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return data is ProjectFileItem;
        }

        public string FormatString(object data)
        {
            return ((ProjectFileItem)data).FilePath;
        }

        public string FormatHtml(object data)
        {
            return ((ProjectFileItem) data).FilePath;
        }
    }
}