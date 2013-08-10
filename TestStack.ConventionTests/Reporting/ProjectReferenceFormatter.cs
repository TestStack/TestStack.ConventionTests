namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class ProjectReferenceFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is ProjectReference;
        }

        public ConventionReportFailure Format(object failingData)
        {
            return new ConventionReportFailure(((ProjectReference)failingData).ReferencedPath);
        }
    }
}