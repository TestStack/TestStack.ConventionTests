namespace TestStack.ConventionTests.ConventionData
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    public class ProjectPropertyGroups : AbstractProjectData
    {
        public ProjectPropertyGroups(Assembly assembly, IProjectProvider projectProvider = null, IProjectLocator projectLocator = null)
            : base(assembly, projectProvider, projectLocator)
        {
        }

        public ProjectPropertyGroup[] PropertyGroups
        {
            get
            {
                var project = GetProject();
                const string msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
                return project
                    .Root
                    .Elements(XName.Get("PropertyGroup", msbuild))
                    .Select(propertyGroup =>
                        new ProjectPropertyGroup(propertyGroup.Attribute("Condition") != null ? propertyGroup.Attribute("Condition").Value : null,
                            propertyGroup.Elements()
                                .Select(item => new KeyValuePair<string, string>(item.Name.LocalName, item.Value)))
                        {

                        })
                    .ToArray();
            }   
        }
    }
}