namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    public class ProjectFiles : AbstractProjectData
    {
        public ProjectFiles(Assembly assembly, IProjectProvider projectProvider, IProjectLocator projectLocator)
            : base(assembly, projectProvider, projectLocator)
        {
            Items = PredicateHelpers.All<ProjectFile>();
        }


        public ProjectFile[] Files
        {
            get
            {
                var project = GetProject();
                const string msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
                return project
                    .Element(XName.Get("Project", msbuild))
                    .Elements(XName.Get("ItemGroup", msbuild))
                    .Elements()
                    .Select(refElem =>
                        new ProjectFile
                        {
                            ReferenceType = refElem.Name.LocalName,
                            FilePath = refElem.Attribute("Include").Value
                        })
                    .Where(Items)
                    .ToArray();
            }
        }

        public Func<ProjectFile, bool> Items { get; set; }
    }
}