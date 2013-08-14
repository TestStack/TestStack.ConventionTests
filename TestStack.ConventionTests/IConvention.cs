namespace TestStack.ConventionTests
{
    public interface IConvention<in T> where T : IConventionData
    {
        void Execute(T data, IConventionResult result);
    }
}