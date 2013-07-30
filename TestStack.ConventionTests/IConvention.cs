namespace TestStack.ConventionTests
{
    using TestStack.ConventionTests.Internal;

    public interface IConvention<T> where T : IConventionData
    {
        ConventionResult Execute(T data);
    }
}