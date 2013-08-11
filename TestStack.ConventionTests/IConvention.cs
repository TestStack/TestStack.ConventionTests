namespace TestStack.ConventionTests
{
    using TestStack.ConventionTests.Internal;

    public interface IConvention<in T> where T : IConventionData
    {
        string ConventionTitle { get; }
        ConventionResult Execute(T data);
    }
}