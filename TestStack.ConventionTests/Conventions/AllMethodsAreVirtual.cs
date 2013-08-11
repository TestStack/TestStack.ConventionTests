namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllMethodsAreVirtual : IConvention<Types>
    {
        public string ConventionTitle
        {
            get { return "Methods must be virtual"; }
        }

        public void Execute(Types data, IConventionResult result)
        {
            result.Is(data.TypesToVerify.SelectMany(t => t.NonVirtualMethods()));
        }
    }
}