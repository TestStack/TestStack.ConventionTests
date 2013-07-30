namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using ApprovalTests;

    public static class Convention
    {
        public static readonly ConventionSettings Settings = new ConventionSettings();

        public static void Is<TData>(IConvention<TData> convention, TData data) where TData : IConventionData
        {
            if (data.HasValidSource == false)
            {
                // TODO: this would have to have a more reasonable and helpful message...
                Settings.AssertInclunclusive("No valid source in " + data);
                return;
            }
            var result = convention.Execute(data);
            if (result.IsConclusive == false)
            {
                Settings.AssertInclunclusive(result.Message);
                return;
            }
            if (data.HasApprovedExceptions)
            {
                // should we encapsulate Approvals behind Settings?
                Approvals.Verify(result.Message);
                return;
            }
            Settings.AssertZero(result.InvalidResultsCount, result.Message);
        }


        public static void Is<T>(ConventionData<T> convention, IEnumerable<T> itemsToVerify)
        {
            var results = Result(convention, itemsToVerify);
            if (!string.IsNullOrEmpty(results))
                throw new ConventionFailedException(results);
        }

        public static void Is<TConvention, T>(TConvention convention, IEnumerable<T> itemsToVerify, Func<string, bool> itemFilter)
            where TConvention : ConventionData<T>, IRuntimeFilter<string>
        {
            Is<TConvention, T, string>(convention, itemsToVerify, itemFilter);
        }

        public static void Is<TConvention, T, TItem>(TConvention convention, IEnumerable<T> itemsToVerify, Func<TItem, bool> itemFilter)
            where TConvention : ConventionData<T>, IRuntimeFilter<TItem>
        {
            var results = Result(convention, itemsToVerify, itemFilter);
            if (!string.IsNullOrEmpty(results))
                throw new ConventionFailedException(results);
        }

        public static string Result<T>(ConventionData<T> convention, IEnumerable<T> itemsToVerify)
        {
            var message = new StringBuilder();
            var invalidItems = itemsToVerify.Where(i => !convention.Must(i)).ToArray();
            if (!invalidItems.Any()) return null;

            message.AppendLine(convention.Description ?? "Convention has failing items");
            foreach (var invalidType in invalidItems)
            {
                message.Append('\t');
                convention.ItemDescription(invalidType, message);
            }

            return message.ToString();
        }

        public static string Result<TConvention, T, TItem>(TConvention convention, IEnumerable<T> itemsToVerify, Func<TItem, bool> itemFilter)
            where TConvention : ConventionData<T>, IRuntimeFilter<TItem>
        {
            convention.SetFilter(itemFilter);
            return Result(convention, itemsToVerify);
        }
    }

    public class ConventionSettings
    {
        public ConventionSettings()
        {
            // TODO: initialize the type;
        }

        public Action<String> AssertInclunclusive;

        public Action<int, string> AssertZero;
    }
}