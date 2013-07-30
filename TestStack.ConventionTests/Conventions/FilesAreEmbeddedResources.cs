namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Helpers;

    public class FilesAreEmbeddedResources : ConventionData<Assembly>, IRuntimeFilter<string>
    {
        readonly IProjectLocator projectLocator;
        readonly IProjectProvider projectProvider;
        Func<string, bool> itemFilter;

        public FilesAreEmbeddedResources() : this(new AssemblyProjectLocator(), new ProjectProvider()) { }
        public FilesAreEmbeddedResources(IProjectLocator projectLocator, IProjectProvider projectProvider)
        {
            this.projectLocator = projectLocator;
            this.projectProvider = projectProvider;

            Must = assembly =>
            {
                var references = GetProjectFiles(assembly);

                return references.All(s => s.Item1 == "EmbeddedResource");
            };
            ItemDescription = (assembly, builder) =>
            {
                var filesNotEmbedded = GetProjectFiles(assembly)
                    .Where(r => r.Item1 != "EmbeddedResource");


                builder.AppendLine(string.Format("{0} has the following files which should be embedded resources:",
                    assembly.GetName().Name));
                foreach (var error in filesNotEmbedded.Select(m => m.Item2 + " which currently has a build action of '" + m.Item1 + "'"))
                {
                    builder.Append('\t');
                    builder.AppendLine(error);
                }
            };
        }

        IEnumerable<Tuple<string, string>> GetProjectFiles(Assembly assembly)
        {
            //TODO Not sure about filters, we can interate on this...
            if (itemFilter == null)
                throw new InvalidOperationException("This convention requires you to provide a filter, which is an overload on Convention.Is");

            const string msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            var resolveProjectFilePath = projectLocator.ResolveProjectFilePath(assembly);
            XDocument projDefinition = projectProvider.LoadProjectDocument(resolveProjectFilePath);
            var references = projDefinition
                .Element(XName.Get("Project", msbuild))
                .Elements(XName.Get("ItemGroup", msbuild))
                .Elements()
                .Select(refElem => Tuple.Create(refElem.Name.LocalName, refElem.Attribute("Include").Value))
                .Where(i => itemFilter(i.Item2));

            return references;
        }

        public void SetFilter(Func<string, bool> filter)
        {
            itemFilter = filter;
        }
    }
}