namespace TestStack.ConventionTests
{
    public interface IConvention<in T> where T : IConventionData
    {
        string ConventionTitle { get; }
        void Execute(T data, IConventionResult result);
    }
}