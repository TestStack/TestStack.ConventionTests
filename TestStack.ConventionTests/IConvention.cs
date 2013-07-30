namespace TestStack.ConventionTests
{
    using TestStack.ConventionTests.Internal;

    public interface IConvention<T>
    {
        ConventionResult Execute(T data);
    }
}