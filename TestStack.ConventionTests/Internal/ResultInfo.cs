namespace TestStack.ConventionTests.Internal
{
    using TestStack.ConventionTests.Reporting;

    public class ResultInfo
    {
        public TestResult Result { get; private set; }
        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public ConventionReportFailure[] ConventionFailures { get; private set; }
        public string ApprovedException { get; private set; }

        public ResultInfo(TestResult result, string conventionTitle, string dataDescription, ConventionReportFailure[] conventionFailures)
        {
            Result = result;
            ConventionTitle = conventionTitle;
            DataDescription = dataDescription;
            ConventionFailures = conventionFailures;
        }

        public void WithApprovedException(string output)
        {
            ApprovedException = output;
            Result = TestResult.Passed;
            ConventionFailures = new ConventionReportFailure[0];
        }
    }
}