namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    public class ProjectDoesNotReferenceDllsFromBinOrObjDirectories : IConvention<Project>
    {
        const string AssemblyReferencingObjRegex = @"^(?<assembly>.*?(obj|bin).*?)$";

        public ConventionResult Execute(Project data)
        {
            var invalid = AllProjectReferences(data.GetProject()).Where(IsBinOrObjReference);
            return ConventionResult.For(invalid, "Some invalid references found.", (r, m) => m.AppendLine("\t" + r));
        }


        static bool IsBinOrObjReference(string reference)
        {
            return Regex.IsMatch(reference, AssemblyReferencingObjRegex, RegexOptions.IgnoreCase);
        }

        static IEnumerable<string> AllProjectReferences(XDocument projDefinition)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            var references = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Reference")
                .Elements(msbuild + "HintPath")
                .Select(refElem => refElem.Value);
            return references;
        }
    }
}