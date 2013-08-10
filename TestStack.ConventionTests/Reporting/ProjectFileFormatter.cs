namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class ProjectFileFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is ProjectFile;
        }

        public ConventionReportFailure Format(object failingData)
        {
            return new ConventionReportFailure(((ProjectFile)failingData).FilePath);
        }
    }
}