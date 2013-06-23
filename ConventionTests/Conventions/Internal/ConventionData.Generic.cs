namespace ConventionTests
{
    using System;
    using System.Text;

    /// <summary>
    ///     This is where we set what our convention is all about
    /// </summary>
    public class ConventionData<TItem>
    {
        public static readonly Predicate<TItem> All = _ => true;
        public static readonly Predicate<TItem> None = _ => false;
        Action<TItem, StringBuilder> DefaultItemDescription = DefaultItemDescriptionMethod;

        static void DefaultItemDescriptionMethod(TItem item, StringBuilder message)
        {
            if (ReferenceEquals(item, null))
            {
                message.AppendLine("<<null>>");
            }
            else
            {
                message.AppendLine(item.ToString());
            }
        }


        public ConventionData(params TItem[] sourceItems)
        {
            SourceItems = sourceItems;
            Items = All;
            Must = None;
            OrderBy = HashCode;
            ItemDescription = DefaultItemDescription;
        }

        public TItem[] SourceItems { get; set; }

        public Func<TItem, object> OrderBy { get; set; }

        /// <summary>
        ///     Descriptive text used for failure message in test. Should explan what is wrong, and how to fix it (how to make types that do not conform to the convention do so).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Specifies that there are valid exceptions to the rule specified by the convention.
        /// </summary>
        /// <remarks>
        ///     When set to <c>true</c> will run the test as Approval test so that the exceptional cases can be reviewed and approved.
        /// </remarks>
        public bool HasApprovedExceptions { get; set; }

        /// <summary>
        ///     This is the convention. The predicate should return <c>true</c> for types that do conform to the convention, and <c>false</c> otherwise
        /// </summary>
        public Predicate<TItem> Must { get; set; }

        /// <summary>
        ///     Predicate that finds types that we want to apply out convention to.
        /// </summary>
        public Predicate<TItem> Items { get; set; }

        public Action<TItem, StringBuilder> ItemDescription { get; set; }

        object HashCode(TItem arg)
        {
            if (ReferenceEquals(arg, null))
            {
                return 0;
            }
            return arg.GetHashCode();
        }

        /// <summary>
        ///     helper method to set <see cref="HasApprovedExceptions" /> in a more convenient manner.
        /// </summary>
        /// <returns> </returns>
        public ConventionData<TItem> WithApprovedExceptions(string explanation = null)
        {
            HasApprovedExceptions = true;
            return this;
        }
    }
}