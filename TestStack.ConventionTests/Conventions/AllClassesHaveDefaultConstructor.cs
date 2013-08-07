namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public AllClassesHaveDefaultConstructor()
        {
            HeaderMessage = "The following types do not have default constructor";
        }

        public string HeaderMessage { get; set; }

        public ConventionResult Execute(Types data)
        {
            var invalid = data.TypesToVerify.Where(t => t.HasDefaultConstructor() == false);
            return ConventionResult.For(invalid, HeaderMessage, t => "\t" + t);
        }
    }
}