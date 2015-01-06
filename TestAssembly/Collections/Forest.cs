namespace TestAssembly.Collections
{
    using System.Collections;
    using System.Collections.Generic;

    public class Forest : ICanAdd<Tree>, ICanRemove<Tree>
    {
        readonly List<Tree> items = new List<Tree>();

        public void Add(Tree item)
        {
            items.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) items).GetEnumerator();
        }

        public IEnumerator<Tree> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool Remove(Tree item)
        {
            return items.Remove(item);
        }
    }
}