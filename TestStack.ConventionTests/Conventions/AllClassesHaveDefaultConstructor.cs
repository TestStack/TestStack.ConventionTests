namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public string ConventionTitle
        {
            get { return "Types must have a default constructor"; }
        }

        public void Execute(Types data, IConventionResult result)
        {
            result.Is(data.TypesToVerify.Where(t => t.HasDefaultConstructor() == false));
        }
    }
}