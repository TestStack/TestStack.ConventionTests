namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllMethodsAreVirtual : IConvention<Types>
    {
        public void Execute(Types data, IConventionResultContext result)
        {
            result.Is("Methods must be virtual", data.TypesToVerify.SelectMany(t => t.NonVirtualMethods()));
        }
    }
}