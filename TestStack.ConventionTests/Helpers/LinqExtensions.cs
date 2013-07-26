// ReSharper disable once CheckNamespace
namespace System.Linq
{
    using System;
    using System.Collections.Generic;

    public static class LinqExtensions
    {
        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }
 
        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return !enumerable.Any(predicate);
        }
    }
}