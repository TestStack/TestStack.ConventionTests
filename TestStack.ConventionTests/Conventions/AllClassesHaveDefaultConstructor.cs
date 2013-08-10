namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public string ConventionTitle { get { return "Types must have a default constructor"; } }

        public IEnumerable<object> GetFailingData(Types data)
        {
            return data.TypesToVerify.Where(t => t.HasDefaultConstructor() == false);
        }
    }
}