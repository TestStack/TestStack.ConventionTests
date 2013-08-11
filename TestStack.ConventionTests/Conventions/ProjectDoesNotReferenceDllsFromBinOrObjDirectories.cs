namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using TestStack.ConventionTests.ConventionData;

    public class ProjectDoesNotReferenceDllsFromBinOrObjDirectories : Convention<ProjectReferences>
    {
        const string AssemblyReferencingObjRegex = @"^(?<assembly>.*?(obj|bin).*?)$";

        public override string ConventionTitle
        {
            get { return "Project must not reference dlls from bin or obj directories"; }
        }

        protected override IEnumerable<object> Execute(ProjectReferences data)
        {
            return data.References.Where(IsBinOrObjReference);
        }

        static bool IsBinOrObjReference(ProjectReference reference)
        {
            return Regex.IsMatch(reference.ReferencedPath, AssemblyReferencingObjRegex, RegexOptions.IgnoreCase);
        }
    }
}