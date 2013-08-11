namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;

    public interface IConventionResult
    {
        void Is<T>(string resultTitle, IEnumerable<T> failingData);
        void IsSymmetric<T>(
            string firstResultTitle, IEnumerable<T> firstFailingData,
            string secondResultTitle, IEnumerable<T> secondFailingData);
    }
}