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

        public void ThrowIfHasInvalidSource()
        {
            if (ProjectLocator.ResolveProjectFilePath(Assembly) == null)
                throw new ConventionSourceInvalidException("Cannot resolve project file for assembly {0}");
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