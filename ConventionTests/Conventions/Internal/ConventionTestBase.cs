namespace ConventionTests
{
    using ApprovalTests;

    public abstract class ConventionTestBase : IConventionTest
    {
        public virtual string Name
        {
            get { return GetType().Name.Replace('_', ' '); }
        }

        public abstract void Execute(IAssert assert);

        protected void Approve(string message)
        {
            Approvals.Verify(new ApprovalTextWriter(message), new ConventionTestNamer(GetType().Name),
                             Approvals.GetReporter());
        }
    }
}