namespace TestStack.ConventionTests.Tests
{
    using System.Xml.Linq;
    using ApprovalTests;
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
        public void assemblies_referencing_bin_obj()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            var ex = Assert.Throws<ConventionFailedException>(() => Convention.Is(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(), project));

            Approvals.Verify(ex.Message);
        }

        [Test]
        public void assemblies_referencing_bin_obj_with_approved_exceptions()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            Convention.IsWithApprovedExeptions(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(), project);
        }

        [Test]
        public void scripts_not_embedded_resources()
        {
            project.Includes = i => i.EndsWith(".sql");
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));

            var ex = Assert.Throws<ConventionFailedException>(() => Convention.Is(new FilesAreEmbeddedResources(), project));

            Approvals.Verify(ex.Message);
        }

        [Test]
        public void scripts_not_embedded_resources_with_approved_exceptions()
        {
            project.Includes = i => i.EndsWith(".sql");
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));

            Convention.IsWithApprovedExeptions(new FilesAreEmbeddedResources(), project);
        }
    }
}