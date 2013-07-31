namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
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
            var invalid = data.ApplicableTypes.ToLookup(t => t, t => t.NonVirtualMethods()).Where(l => l.Any());
            return ConventionResult.For(invalid, HeaderMessage, DescribeTypeAndMethods);
        }

        // I like how that's encapsulated in the reusable convention type, whereas previously it was part of the convention/test code
        void DescribeTypeAndMethods(IGrouping<Type, IEnumerable<MethodInfo>> item, StringBuilder message)
        {
            message.AppendLine("\t" + item.Key);
            foreach (var method in item)
            {
                message.AppendLine("\t\t" + method);
            }
        }
    }
}