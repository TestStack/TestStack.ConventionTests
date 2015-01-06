namespace TestAssembly.Collections
{
    using System.Collections.Generic;

    public interface ICanAdd<T> : IEnumerable<T>
    {
        void Add(T item);
    }
}