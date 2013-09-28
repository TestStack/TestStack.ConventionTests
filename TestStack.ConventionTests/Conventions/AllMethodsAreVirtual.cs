namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class AllMethodsAreVirtual : IConvention<Types>
    {
        public void Execute(Types data, IConventionResultContext result)
        {
            result.Is("Methods must be virtual", data.TypesToVerify.SelectMany(t => t.NonVirtualMethods()));
        }

        public string ConventionReason
        {
            get { return "This convention is useful when classes need to be proxied (nHibernate entities), which need members to be virtual"; }
        }
    }
}