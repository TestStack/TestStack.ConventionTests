namespace TestStack.ConventionTests
{
    using System;
    using System.Text;

    /// <summary>
    ///     This is where we set what our convention is all about
    /// </summary>
    public class ConventionData<TItem>
    {
        public static readonly Predicate<TItem> None = _ => false;
        readonly Action<TItem, StringBuilder> defaultItemDescription = DefaultItemDescriptionMethod;

        public ConventionData()
        {
            Must = None;
            OrderBy = HashCode;
            ItemDescription = defaultItemDescription;
        }

        public Func<TItem, object> OrderBy { get; set; }

        /// <summary>
        ///     Descriptive text used for failure message in test. Should explan what is wrong, and how to fix it (how to make
        ///     types that do not conform to the convention do so).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     This is the convention. The predicate should return <c>true</c> for types that do conform to the convention, and
        ///     <c>false</c> otherwise
        /// </summary>
        public Predicate<TItem> Must { get; set; }

        public Action<TItem, StringBuilder> ItemDescription { get; set; }

        static void DefaultItemDescriptionMethod(TItem item, StringBuilder message)
        {
            message.AppendLine(ReferenceEquals(item, null) ? "<<null>>" : item.ToString());
        }

        object HashCode(TItem arg)
        {
            if (ReferenceEquals(arg, null))
            {
                return 0;
            }
            return arg.GetHashCode();
        }
    }
}