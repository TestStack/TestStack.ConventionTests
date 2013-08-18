namespace TestStack.ConventionTests.Internal
{
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public class ConventionResult
    {
        public ConventionResult(string conventionTitle, string dataDescription, object[] data)
        {
            ConventionTitle = conventionTitle;
            DataDescription = dataDescription;
            Data = data;
        }

        public TestResult Result
        {
            get
            {
                if (Data.Any())
                {
                    return TestResult.Failed;
                }
                return TestResult.Passed;
            }
        }

        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public object[] Data { get; private set; }
    }
}