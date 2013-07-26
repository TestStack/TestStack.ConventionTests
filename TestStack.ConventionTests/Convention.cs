namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class Convention
    {
        public static void Is<T, T2>(IEnumerable<T2> itemsToVerify) where T : ConventionData<T2>, new()
        {
            Is(new T(), itemsToVerify);
        }
        public static void Is<T, T2>(IEnumerable<T2> itemsToVerify, Func<string, bool> filter)
            where T : ConventionData<T2>, IRuntimeFilter<string>, new()
        {
            Is(new T(), itemsToVerify);
        }
        public static void Is<T, T2, TItem>(IEnumerable<T2> itemsToVerify, Func<TItem, bool> filter) 
            where T : ConventionData<T2>, IRuntimeFilter<TItem>, new()
        {
            Is(new T(), itemsToVerify);
        }

        //Item.Is<ConventionData<Type>>
        public static void Is<T>(IEnumerable<Type> itemsToVerify) where T : ConventionData<Type>, new()
        {
            Is(new T(), itemsToVerify);
        }
        public static void Is<T>(IEnumerable<Type> itemsToVerify, Func<string, bool> filter) where T : ConventionData<Type>, IRuntimeFilter<string>, new()
        {
            Is(new T(), itemsToVerify, filter);
        }
        public static void Is<T, TItem>(IEnumerable<Type> itemsToVerify, Func<TItem, bool> filter) where T : ConventionData<Type>, IRuntimeFilter<TItem>, new()
        {
            Is(new T(), itemsToVerify, filter);
        }

        //Item.Is<ConventionData<Assembly>>
        public static void Is<T>(IEnumerable<Assembly> itemsToVerify) where T : ConventionData<Assembly>, new()
        {
            Is(new T(), itemsToVerify);
        }
        public static void Is<T>(IEnumerable<Assembly> itemsToVerify, Func<string, bool> filter) where T : ConventionData<Assembly>, IRuntimeFilter<string>, new()
        {
            Is(new T(), itemsToVerify, filter);
        }
        public static void Is<T, TItem>(IEnumerable<Assembly> itemsToVerify, Func<TItem, bool> filter) where T : ConventionData<Assembly>, IRuntimeFilter<TItem>, new()
        {
            Is(new T(), itemsToVerify, filter);
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
}