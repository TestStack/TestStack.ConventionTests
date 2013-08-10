namespace TestStack.ConventionTests.Internal
{
    public class ConventionReportFailure
    {
        public string Failure { get; set; }

        public ConventionReportFailure(string failure)
        {
            Failure = failure;
        }

        public override string ToString()
        {
            return Failure;
        }
    }
}