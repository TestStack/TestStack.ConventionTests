namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.Helpers;

    public class AllMethodsAreVirtual : ConventionData
    {
        public AllMethodsAreVirtual()
        {
            Must = t => t.NonVirtualMethods().None();
            ItemDescription = (type, builder) =>
            {
                builder.Append(type.FullName);
                builder.AppendLine(" has non virtual method(s):");
                foreach (var nonVirtual in type.NonVirtualMethods())
                {
                    builder.Append('\t');
                    builder.AppendLine(nonVirtual.Name);
                }
            };
        }
    }
}