namespace ConventionTests
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     Base class for convention tests. Inherited types should be put in "/Conventions" folder in test assembly and follow Sentence_naming_convention_with_underscores_indead_of_spaces These tests will be ran by
    ///     <see
    ///         cref="ConventionTestsRunner" />
    ///     .
    /// </summary>
    public abstract class ConventionTest<TItem> : ConventionTestBase
    {
        public override void Execute(IAssert assert)
        {
            var data = SetUp();
            var itemsToTest = GetItemsToTest(data);
            if (itemsToTest.Length == 0)
            {
                assert.Inconclusive(
                    "No items found to apply the convention to. Make sure the Items predicate is correct and that the right sourceItems are specified.");
            }
            var invalidItems = Array.FindAll(itemsToTest, t => data.Must(t) == false);
            var message = new StringBuilder();
            message.AppendLine(data.Description ?? "Invalid items found");
            foreach (var invalidType in invalidItems)
            {
                message.Append('\t');
                data.ItemDescription(invalidType, message);
            }
            if (data.HasApprovedExceptions)
            {
                Approve(message.ToString());
            }
            else
            {
                assert.AreEqual(0, invalidItems.Count(), message.ToString());
            }
        }

        /// <summary>
        ///     This is the only method you need to override. Return a <see cref="ConventionData" /> that describes your convention.
        /// </summary>
        /// <returns> </returns>
        protected abstract ConventionData<TItem> SetUp();

        protected virtual TItem[] GetItemsToTest(ConventionData<TItem> data)
        {
            return data.SourceItems
                       .Where(data.Items.Invoke)
                       .OrderBy(data.OrderBy)
                       .ToArray();
        }
    }
}