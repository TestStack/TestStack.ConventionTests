namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Internal;

    public class Project : IConventionData
    {
        public Func<string, bool> Includes;

        public Project(Assembly assembly, IProjectProvider projectProvider, IProjectLocator projectLocator)
        {
            Assembly = assembly;
            ProjectProvider = projectProvider;
            ProjectLocator = projectLocator;
        }

        public Assembly Assembly { get; private set; }

        public IProjectLocator ProjectLocator { get; private set; }

        public IProjectProvider ProjectProvider { get; private set; }

        public bool HasValidSource
        {
            get { return ProjectLocator.ResolveProjectFilePath(Assembly) != null; }
        }

        public bool HasApprovedExceptions { get; set; }

        public XDocument GetProject()
        {
            var location = ProjectLocator.ResolveProjectFilePath(Assembly);
            var project = ProjectProvider.LoadProjectDocument(location);
            return project;
        }
    }
}