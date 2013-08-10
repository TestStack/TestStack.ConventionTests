namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;

    public interface ISymmetricConvention<in T> where T : IConventionData
    {
        string ConventionTitle { get; }
        string InverseTitle { get; }
        IEnumerable<object> GetFailingData(T data);
        IEnumerable<object> GetFailingInverseData(T data);
    }
}