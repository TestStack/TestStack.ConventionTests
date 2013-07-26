namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Helpers;

    public class ProjectDoesNotReferenceDllsFromBinOrObjDirectories : ConventionData<Assembly>
    {
        const string AssemblyReferencingObjRegex = @"^(?<assembly>.*?(obj|bin).*?)$";

        public ProjectDoesNotReferenceDllsFromBinOrObjDirectories()
            : this(new AssemblyProjectLocator(), new ProjectProvider())
        {
        }

        public ProjectDoesNotReferenceDllsFromBinOrObjDirectories(IProjectLocator projectLocator, IProjectProvider projectProvider)
        {
            Must = assembly =>
            {
                var references = GetProjectReferences(projectLocator, projectProvider, assembly);

                return references.All(s => !Regex.IsMatch(s, AssemblyReferencingObjRegex, RegexOptions.IgnoreCase));
            };
            ItemDescription = (assembly, builder) =>
            {
                var first = GetProjectReferences(projectLocator, projectProvider, assembly)
                    .Select(r => Regex.Match(r, AssemblyReferencingObjRegex, RegexOptions.IgnoreCase))
                    .First(r => r.Success);

                var message = string.Format(
                    "{0} is referencing assemblies in the bin or obj folders, {1}",
                    assembly.GetName().Name,
                   first.Groups["assembly"].Value);
                builder.AppendLine(message);
            };
        }

        static IEnumerable<string> GetProjectReferences(IProjectLocator projectLocator, IProjectProvider projectProvider,
            Assembly assembly)
        {
            XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            var resolveProjectFilePath = projectLocator.ResolveProjectFilePath(assembly);
            XDocument projDefinition = projectProvider.LoadProjectDocument(resolveProjectFilePath);
            IEnumerable<string> references = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Reference")
                .Elements(msbuild + "HintPath")
                .Select(refElem => refElem.Value);
            return references;
        }
    }
}