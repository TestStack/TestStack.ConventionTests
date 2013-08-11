namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllMethodsAreVirtual : Convention<Types>
    {
        public override string ConventionTitle
        {
            get { return "Methods must be virtual"; }
        }

        protected override IEnumerable<object> Execute(Types data)
        {
            return data.TypesToVerify.SelectMany(t => t.NonVirtualMethods());
        }
    }
}