namespace ConventionTests
{
    public interface IConventionTest
    {
        string Name { get; }
        void Execute(IAssert assert);
    }
}