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
            string firstHeader, TResult[] firstResults,
            string secondHeader, TResult[] secondResults,
            Action<TResult, StringBuilder> itemDescriptor);

        void IsSymmetric<TResult>(
            string firstHeader, TResult[] firstResults,
            string secondHeader, TResult[] secondResults,
            Func<TResult, string> itemDescriptor);
    }
}