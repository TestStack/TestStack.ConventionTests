namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public void Execute(Types data, IConventionResultContext result)
        {
            result.Is("Types must have a default constructor",
                data.TypesToVerify.Where(t => t.HasDefaultConstructor() == false));
        }

        public string ConventionReason
        {
            get { return "This convention is useful when classes need to be proxied (nHibernate/Entity Framework entities), which need a public or protected constructor"; }
        }
    }
}