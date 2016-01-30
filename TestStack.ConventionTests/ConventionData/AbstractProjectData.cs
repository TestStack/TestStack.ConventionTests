namespace TestStack.ConventionTests.ConventionData
{
    using System.IO;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    public abstract class AbstractProjectData : IConventionData
    {
        protected AbstractProjectData(IProjectProvider projectProvider)
        {
            ProjectProvider = projectProvider;
        }
        protected AbstractProjectData(string projectFilePath)
        {
            ProjectProvider = new ProjectFileFromDiskProvider(projectFilePath);
        }

        public IProjectProvider ProjectProvider { get; private set; }

        public string Description { get { return ProjectProvider.GetName(); } }

        public bool HasData { get { return true; } }

        protected XDocument GetProject()
        {
            return ProjectProvider.LoadProjectDocument();
        }
    }
}