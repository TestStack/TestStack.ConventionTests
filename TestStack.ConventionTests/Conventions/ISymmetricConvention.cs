namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;

    public interface ISymmetricConvention<in T, out TDataType> where T : IConventionData
    {
        string ConventionTitle { get; }
        string InverseTitle { get; }
        IEnumerable<TDataType> GetFailingData(T data);
        IEnumerable<TDataType> GetFailingInverseData(T data);
    }
}