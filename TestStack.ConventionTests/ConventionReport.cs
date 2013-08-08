namespace TestStack.ConventionTests
{
    using System.Collections.Generic;
    using System.Linq;

    public class ConventionReport
    {
        public Result Result { get; private set; }
        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public IEnumerable<ConventionFailure> ConventionFailures { get; private set; }
        public string ApprovedException { get; private set; }

        public ConventionReport(Result result, string conventionTitle, string dataDescription, IEnumerable<ConventionFailure> conventionFailures)
        {
            Result = result;
            ConventionTitle = conventionTitle;
            DataDescription = dataDescription;
            ConventionFailures = conventionFailures;
        }

        public void WithApprovedException(string output)
        {
            ApprovedException = output;
            Result = Result.Passed;
            ConventionFailures = Enumerable.Empty<ConventionFailure>();
        }
    }
}