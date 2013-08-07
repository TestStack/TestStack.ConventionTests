namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class ProjectDoesNotReferenceDllsFromBinOrObjDirectories : IConvention<ProjectReferences>
    {
        const string AssemblyReferencingObjRegex = @"^(?<assembly>.*?(obj|bin).*?)$";

        public ConventionResult Execute(ProjectReferences data)
        {
            var invalid = data.References.Where(IsBinOrObjReference);
            var header = string.Format("Some invalid assembly references found in {0}", data.Assembly.GetName().Name);
            return ConventionResult.For(invalid, header, FormatLine);
        }

        void FormatLine(ProjectReference assemblyReference, StringBuilder m)
        {
            m.AppendLine("\t" + assemblyReference.ReferencedPath);
        }

        static bool IsBinOrObjReference(ProjectReference reference)
        {
            return Regex.IsMatch(reference.ReferencedPath, AssemblyReferencingObjRegex, RegexOptions.IgnoreCase);
        }
    }
}