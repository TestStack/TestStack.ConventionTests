namespace TestStack.ConventionTests
{
    using System.Collections.Generic;

    public interface IConvention<in T> where T : IConventionData
    {
        string ConventionTitle { get; }
        IEnumerable<object> GetFailingData(T data);
    }
}