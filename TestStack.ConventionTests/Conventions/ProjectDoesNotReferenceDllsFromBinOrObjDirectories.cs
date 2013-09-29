namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using TestStack.ConventionTests.ConventionData;

    public class ProjectDoesNotReferenceDllsFromBinOrObjDirectories : IConvention<ProjectReferences>
    {
        const string AssemblyReferencingObjRegex = @"^(?<assembly>.*?(obj|bin).*?)$";

        public void Execute(ProjectReferences data, IConventionResultContext result)
        {
            result.Is("Project must not reference dlls from bin or obj directories",
                data.References.Where(IsBinOrObjReference));
        }

        public string ConventionReason { get { return "Referencing assemblies from the bin/obj folder is normally due to a broken reference, this convention detects these issues"; } }

        static bool IsBinOrObjReference(ProjectReference reference)
        {
            return Regex.IsMatch(reference.ReferencedPath, AssemblyReferencingObjRegex, RegexOptions.IgnoreCase);
        }
    }
}