namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IConventionResult
    {
        void Is<TResult>(IEnumerable<TResult> items);

        void Is<TResult>(
            IEnumerable<TResult> items,
            Action<TResult, StringBuilder> itemDescriptor);

        void Is<TResult>(
            IEnumerable<TResult> items,
            Func<TResult, string> itemDescriptor);

        void IsSymmetric<TResult>(
            IEnumerable<TResult> items,
            Func<TResult, bool> firstPredicate,
            Func<TResult, bool> secondPredicate,
            string firstDescription = null,
            string secondDescription = null);
    }
}