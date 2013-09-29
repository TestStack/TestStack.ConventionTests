namespace TestStack.ConventionTests.Tests.TestConventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestAssembly.Collections;
    using TestStack.ConventionTests.ConventionData;

    public class CollectionsRelationsConvention : IConvention<Types>
    {
        public string ConventionTitle { get; private set; }

        public void Execute(Types data, IConventionResultContext result)
        {
            ConventionTitle = "well, does the header apply here all across the board? How would that work for CSV?";
            var types = data.TypesToVerify;
            var collectionToItemLookup = from collection in types
                where collection.IsClass
                orderby collection.FullName
                from item in GetItemTypes(collection)
                select new
                {
                    collection,
                    item,
                    can_add = typeof (ICanAdd<>).MakeGenericType(item).IsAssignableFrom(collection),
                    can_remove = typeof (ICanRemove<>).MakeGenericType(item).IsAssignableFrom(collection)
                };

            result.Is("Some title", collectionToItemLookup);
        }

        public string ConventionReason
        {
            get { return "Test convention"; }
        }

        IEnumerable<Type> GetItemTypes(Type type)
        {
            return from @interface in type.GetInterfaces()
                where @interface.IsGenericType
                where @interface.GetGenericTypeDefinition() == typeof (IEnumerable<>)
                let item = @interface.GetGenericArguments().Single()
                orderby item.FullName
                select item;
        }
    }
}