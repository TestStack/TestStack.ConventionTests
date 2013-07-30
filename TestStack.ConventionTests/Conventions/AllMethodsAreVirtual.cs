namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using TestStack.ConventionTests.Helpers;
    using TestStack.ConventionTests.Internal;

    public class AllMethodsAreVirtual : IConvention<Types>
    {
        public AllMethodsAreVirtual()
        {
            HeaderMessage = "The following methods are not virtual.";
        }

        public ConventionResult Execute(Types data)
        {
            var types = data.ApplicableTypes;
            if (types.None())
            {
                return ConventionResult.Inconclusive("Put sensible 'inconclusive' message here");
            }

            // do we want to encapsulate that in some way?
            // also notice how data gives us types, yet the convention acts upon methods.
            var invalid = types.ToLookup(t => t, t => t.NonVirtualMethods()).Where(l => l.Any());
            var result = ConventionResult.For(invalid, HeaderMessage, DescribeTypeAndMethods);
            result.HasExceptions = data.HasApprovedExceptions;
            return result;
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

        public string HeaderMessage { get; set; }
    }
}