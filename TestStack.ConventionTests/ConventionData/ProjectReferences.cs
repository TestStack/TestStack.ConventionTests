namespace TestStack.ConventionTests.ConventionData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    public class ProjectReferences : AbstractProjectData
    {
        public ProjectReferences(Assembly assembly, IProjectProvider projectProvider, IProjectLocator projectLocator)
            : base(assembly, projectProvider, projectLocator)
        {
        }

        public ProjectReference[] References
        {
            get
            {
                var project = GetProject();
                return AllProjectReferences(project)
                    .Select(r => new ProjectReference
                    {
                        ReferencedPath = r
                    })
                    .ToArray();
            }
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