namespace ConventionTests
{
    using System;

    public class ClassHasDefaultConstructorConvention : ConventionData
    {
        public ClassHasDefaultConstructorConvention(params Type[] sourceTypes) : base(sourceTypes)
        {
            Must = type => type.HasDefaultConstructor();
            ItemDescription =
                (type, builder) => builder.AppendLine(string.Format("{0} does not have a default constructor", type.FullName));
        }
    }
}