namespace TestStack.ConventionTests
{
    public interface IConvention<in TData> where TData : IConventionData
    {
        string ConventionTitle { get; }
        void Execute(TData data, IConventionResult result);
    }
}