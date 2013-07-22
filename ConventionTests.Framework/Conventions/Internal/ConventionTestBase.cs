namespace ConventionTests
{
    using System.ComponentModel;
    using ApprovalTests;

    public abstract class ConventionTestBase : IConventionTest
    {
        public virtual string Name
        {
            get
            {
                var descriptions = GetType().GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (descriptions.Length == 1)
                {
                    return ((DescriptionAttribute) descriptions[0]).Description;
                }
                return GetType().Name.Replace('_', ' ');
            }
        }

        public abstract void Execute(IAssert assert);

        protected void Approve(string message)
        {
            Approvals.Verify(new ApprovalTextWriter(message),
                             new ConventionTestNamer(GetType().Name),
                             Approvals.GetReporter());
        }
    }
}