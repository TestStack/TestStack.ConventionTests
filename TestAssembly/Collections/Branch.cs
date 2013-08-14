namespace TestAssembly.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    public class Branch : IEnumerable<Leaf>
    {
        readonly List<Leaf> items = new List<Leaf>();

        public IEnumerator<Leaf> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}