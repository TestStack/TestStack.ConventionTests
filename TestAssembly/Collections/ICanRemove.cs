namespace TestAssembly.Collections
{
    using System.Collections.Generic;

    public interface ICanRemove<T> : IEnumerable<T>
    {
        bool Remove(T item);
    }
}