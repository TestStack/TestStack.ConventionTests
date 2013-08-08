namespace TestStack.ConventionTests
{
    public interface IConventionData
    {
        string Description { get; }
        void EnsureHasNonEmptySource();
    }
}