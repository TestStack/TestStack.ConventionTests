namespace ConventionTests
{
    using System;

    public class AllMethodsAreVirtualConvention : ConventionData
    {
        public AllMethodsAreVirtualConvention(params Type[] sourceTypes)
            : base(sourceTypes)
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