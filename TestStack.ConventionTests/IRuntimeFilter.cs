namespace TestStack.ConventionTests
{
    using System;

    public interface IRuntimeFilter<out T>
    {
        void SetFilter(Func<T, bool> predicate);
    }
}