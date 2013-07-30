namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.Helpers;
    using TestStack.ConventionTests.Internal;

    public class AllClassesHaveDefaultConstructor : IConvention<Types>
    {
        public string HeaderMessage { get; set; }

        public AllClassesHaveDefaultConstructor()
        {
            HeaderMessage = "The following types do not have default constructor";
        }

        public ConventionResult Execute(Types data)
        {
            var types = data.ApplicableTypes;
            if (types.None())
            {
                return ConventionResult.Inconclusive("Put sensible 'inconclusive' message here");
            }
            // NOTE: thie bit above is duplicated in both conventions we have, and will likely be duplicated in any other.
            // we likely will want to pull that out, leaving perhaps a property like InconclusiveMessage

            var invalid = types.Where(t => t.HasDefaultConstructor() == false);
            var result = ConventionResult.For(invalid, HeaderMessage, (t, m) => m.AppendLine("\t" + t));

            //if we switch conventionData to have an interface and put HasApprovedExceptions on it, we'll be able to pull this out as well.
            result.HasExceptions = data.HasApprovedExceptions;
            return result;
        }
    }
}