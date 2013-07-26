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

        public static void Is<T>(IEnumerable<Type> itemsToVerify) where T : ConventionData<Type>, new()
        {
            Is(new T(), itemsToVerify);
        }

        public static void Is<T>(IEnumerable<Assembly> itemsToVerify) where T : ConventionData<Assembly>, new()
        {
            Is(new T(), itemsToVerify);
        }

        public static void Is<T>(ConventionData<T> convention, IEnumerable<T> itemsToVerify)
        {
            var results = Result(convention, itemsToVerify);
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
    }
}