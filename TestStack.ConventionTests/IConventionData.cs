namespace TestStack.ConventionTests
{
    public interface IConventionData
    {
        void ThrowIfHasInvalidSource();
        bool HasApprovedExceptions { get; }
    }
}