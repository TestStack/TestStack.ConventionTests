namespace TestStack.ConventionTests
{
    using System;
    using System.Reflection;
    using System.Xml.Linq;
    using TestStack.ConventionTests.Helpers;

    public class Project : IConventionData
    {
        readonly Assembly assembly;
        readonly IProjectLocator projectLocator;
        readonly IProjectProvider projectProvider;

        public Project(Assembly assembly, IProjectProvider projectProvider, IProjectLocator projectLocator)
        {
            this.assembly = assembly;
            this.projectProvider = projectProvider;
            this.projectLocator = projectLocator;
        }

        public bool HasValidSource
        {
            get { return projectLocator.ResolveProjectFilePath(assembly) != null; }
        }

        public bool HasApprovedExceptions { get; set; }

        public XDocument GetProject()
        {
            var location = projectLocator.ResolveProjectFilePath(assembly);
            var project = projectProvider.LoadProjectDocument(location);
            return project;
        }

        public Func<string, bool> Includes;
    }
}