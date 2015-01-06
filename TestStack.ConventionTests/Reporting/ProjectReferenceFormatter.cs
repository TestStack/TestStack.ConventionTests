namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;

    public class ProjectReferenceFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return data is ProjectReference;
        }

        public string FormatString(object data)
        {
            return ((ProjectReference)data).ReferencedPath;
        }

        public string FormatHtml(object data)
        {
            return ((ProjectReference)data).ReferencedPath;
        }
    }
}