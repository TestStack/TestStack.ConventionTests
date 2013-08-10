namespace TestStack.ConventionTests.ConventionData
{
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;

    public abstract class AbstractProjectData : IConventionData
    {
        protected AbstractProjectData(Assembly assembly, IProjectProvider projectProvider, IProjectLocator projectLocator)
        {
            Assembly = assembly;
            ProjectProvider = projectProvider;
            ProjectLocator = projectLocator;
        }

        public Assembly Assembly { get; private set; }

        public IProjectLocator ProjectLocator { get; private set; }

        public IProjectProvider ProjectProvider { get; private set; }

        public string Description { get { return Assembly.GetName().Name; } }

        public bool HasData { get { return ProjectLocator.ResolveProjectFilePath(Assembly) != null; } }

        protected XDocument GetProject()
        {
            var location = ProjectLocator.ResolveProjectFilePath(Assembly);
            var project = ProjectProvider.LoadProjectDocument(location);
            return project;
        }
    }
}