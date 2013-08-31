namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;

    public class ProjectReferenceFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is ProjectReference;
        }

        public string FormatString(object failingData)
        {
            return ((ProjectReference)failingData).ReferencedPath;
        }
    }
}