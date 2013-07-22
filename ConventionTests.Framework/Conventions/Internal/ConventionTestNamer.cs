namespace ConventionTests
{
    using ApprovalTests.Core;
    using ApprovalTests.Namers;

    public class ConventionTestNamer : UnitTestFrameworkNamer, IApprovalNamer
    {
        readonly string name;

        public ConventionTestNamer(string name)
        {
            this.name = name;
        }

        string IApprovalNamer.Name
        {
            get { return name; }
        }
    }
}