namespace TestStack.ConventionTests
{
    public class ConventionFailure
    {
        public string Failure { get; set; }

        public ConventionFailure(string failure)
        {
            Failure = failure;
        }

        public override string ToString()
        {
            return Failure;
        }
    }
}