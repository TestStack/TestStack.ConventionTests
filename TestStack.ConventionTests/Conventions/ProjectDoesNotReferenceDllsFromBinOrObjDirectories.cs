namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using TestStack.ConventionTests.ConventionData;

    public class ProjectDoesNotReferenceDllsFromBinOrObjDirectories : IConvention<ProjectReferences>
    {
        const string AssemblyReferencingObjRegex = @"^(?<assembly>.*?(obj|bin).*?)$";

        static bool IsBinOrObjReference(ProjectReference reference)
        {
            return Regex.IsMatch(reference.ReferencedPath, AssemblyReferencingObjRegex, RegexOptions.IgnoreCase);
        }

        public string ConventionTitle { get { return "Project must not reference dlls from bin or obj directories"; } }

        public IEnumerable<object> GetFailingData(ProjectReferences data)
        {
            return data.References.Where(IsBinOrObjReference);
        }
    }
}