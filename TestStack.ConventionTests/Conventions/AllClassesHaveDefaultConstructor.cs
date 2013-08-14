namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public void Execute(Types data, IConventionResultContext result)
        {
            result.Is("Types must have a default constructor",
                data.TypesToVerify.Where(t => t.HasDefaultConstructor() == false));
        }
    }
}