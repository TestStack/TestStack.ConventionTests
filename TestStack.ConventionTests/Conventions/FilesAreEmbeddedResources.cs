namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    public class FilesAreEmbeddedResources : IConvention<Project>
    {
        public ConventionResult Execute(Project data)
        {
            return ConventionResult.For(GetProjectFiles(data).Where(s => s.Item1 != "EmbeddedResource"),
                "The following files which should be embedded resources:",
                (t, m) => m.AppendLine("\t" + t.Item2));
        }

        IEnumerable<Tuple<string, string>> GetProjectFiles(Project project)
        {
            if (project.Includes == null)
                throw new InvalidOperationException(
                    "This convention requires you to provide a filter on the convention data");
            const string msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            var projDefinition = project.GetProject();
            var references = projDefinition
                .Element(XName.Get("Project", msbuild))
                .Elements(XName.Get("ItemGroup", msbuild))
                .Elements()
                .Select(refElem => Tuple.Create(refElem.Name.LocalName, refElem.Attribute("Include").Value))
                .Where(i => project.Includes(i.Item2));

            return references;
        }
    }
}