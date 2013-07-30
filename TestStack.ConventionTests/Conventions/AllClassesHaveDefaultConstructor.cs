namespace TestStack.ConventionTests.Conventions
{
    using TestStack.ConventionTests.Helpers;

    public class AllClassesHaveDefaultConstructor : ConventionData
    {
        public AllClassesHaveDefaultConstructor()
        {
            Must = type => type.HasDefaultConstructor();
            ItemDescription = (type, builder) => 
                builder.AppendLine(string.Format("{0} does not have a default constructor", type.FullName));
        }
    }
}