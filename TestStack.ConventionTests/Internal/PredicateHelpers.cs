namespace TestStack.ConventionTests.Internal
{
    using System;

    public static class PredicateHelpers
    {
        public static Func<T, bool> All<T>()
        {
            return _ => true;
        }

        public static Func<T, bool> None<T>()
        {
            return _ => false;
        }
    }
}