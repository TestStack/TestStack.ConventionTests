namespace TestStack.ConventionTests.Tests
{
    using System.Xml.Linq;
    using ApprovalTests.Reporters;
    using NSubstitute;
    using NUnit.Framework;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Tests.Properties;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class ProjectBasedConventions
    {
        Project project;
        IProjectProvider projectProvider;

        [SetUp]
        public void Setup()
        {
            projectProvider = Substitute.For<IProjectProvider>();
            var projectLocator = Substitute.For<IProjectLocator>();
            project = new Project(typeof(ProjectBasedConventions).Assembly, projectProvider, projectLocator);
        }

        [Test]
        public void ReferencingBinObj()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            Convention.Is(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(), project);
        }

        [Test]
        public void ScriptsNotEmbeddedResources()
        {
            project.Includes = i => i.EndsWith(".sql");
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));

            Convention.Is(new FilesAreEmbeddedResources(), project);
        }
    }
}