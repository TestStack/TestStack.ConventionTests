namespace TestStack.ConventionTests
{
    using System.Collections.Generic;

    public interface IConvention<in T, out TDataType> where T : IConventionData
    {
        string ConventionTitle { get; }
        IEnumerable<TDataType> GetFailingData(T data);
    }
}