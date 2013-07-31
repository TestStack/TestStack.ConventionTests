namespace TestStack.ConventionTests
{
    using TestStack.ConventionTests.Internal;

    public interface IConvention<in T> where T : IConventionData
    {
        ConventionResult Execute(T data);
    }
}