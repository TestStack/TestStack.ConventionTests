namespace TestStack.ConventionTests
{
    public interface IConventionData
    {
        bool HasValidSource { get; }
        bool HasApprovedExceptions { get; }
    }
}