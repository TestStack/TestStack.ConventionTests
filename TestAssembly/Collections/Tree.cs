namespace TestAssembly.Collections
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Tree : ICanAdd<Branch>, IEnumerable<Leaf>
    {
        readonly List<Branch> items = new List<Branch>();

        public void Add(Branch item)
        {
            items.Add(item);
        }

        public IEnumerator<Branch> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<Leaf> IEnumerable<Leaf>.GetEnumerator()
        {
            return items.SelectMany(i => i).GetEnumerator();
        }
    }
}