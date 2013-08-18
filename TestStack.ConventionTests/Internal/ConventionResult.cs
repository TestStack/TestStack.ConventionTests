namespace TestStack.ConventionTests.Internal
{
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public class ConventionResult
    {
        public ConventionResult(string conventionTitle, string dataDescription, ConventionReportFailure[] conventionFailures)
        {
            ConventionTitle = conventionTitle;
            DataDescription = dataDescription;
            ConventionFailures = conventionFailures;
        }

        public TestResult Result
        {
            get
            {
                if (ConventionFailures.Any())
                {
                    return TestResult.Failed;
                }
                return TestResult.Passed;
            }
        }

        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public ConventionReportFailure[] ConventionFailures { get; private set; }
        public string ApprovedException { get; private set; }
    }
}