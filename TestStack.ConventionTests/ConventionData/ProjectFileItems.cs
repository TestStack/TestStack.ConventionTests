namespace TestStack.ConventionTests.ConventionData
{
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    /// <summary>
    /// Items/Files in a .*proj project file
    /// </summary>
    public class ProjectFileItems : AbstractProjectData
    {
        public ProjectFileItems(Assembly assembly, IProjectProvider projectProvider = null, IProjectLocator projectLocator = null)
            : base(assembly, projectProvider, projectLocator)
        {
        }

        public ProjectFileItem[] Items
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
                        new ProjectFileItem
                        {
                            ReferenceType = refElem.Name.LocalName,
                            FilePath = refElem.Attribute("Include").Value
                        })
                    .ToArray();
            }
        }
    }
}