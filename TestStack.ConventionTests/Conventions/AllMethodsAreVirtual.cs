namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllMethodsAreVirtual : IConvention<Types>
    {
        public AllMethodsAreVirtual()
        {
            HeaderMessage = "The following methods are not virtual.";
        }

        public string HeaderMessage { get; set; }

        public ConventionResult Execute(Types data)
        {
            // do we want to encapsulate that in some way?
            // also notice how data gives us types, yet the convention acts upon methods.
            var items = from applicableType in data.TypesToVerify
                        let nonVirtuals = applicableType.NonVirtualMethods()
                        where nonVirtuals.Any()
                        select Tuple.Create(applicableType, nonVirtuals);
            return ConventionResult.For(items, HeaderMessage, DescribeTypeAndMethods);
        }

        // I like how that's encapsulated in the reusable convention type, whereas previously it was part of the convention/test code
        void DescribeTypeAndMethods(Tuple<Type, IEnumerable<MethodInfo>> item, StringBuilder message)
        {
            foreach (var method in item.Item2)
            {
                message.Append("\t");
                message.Append(item.Item1);
                message.Append(".");
                message.Append(method.Name);
                message.AppendLine();
            }
        }
    }
}