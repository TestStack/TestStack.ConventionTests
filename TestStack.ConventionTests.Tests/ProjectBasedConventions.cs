namespace TestStack.ConventionTests.Tests
{
    using System.Xml.Linq;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using NSubstitute;
    using NUnit.Framework;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Tests.Properties;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class ProjectBasedConventions
    {
        IProjectProvider projectProvider;

        [SetUp]
        public void Setup()
        {
            projectProvider = Substitute.For<IProjectProvider>();
        }

        [Test]
        public void assemblies_referencing_bin_obj()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            var projectLocator = Substitute.For<IProjectLocator>();
            var project = new ProjectReferences(typeof(ProjectBasedConventions).Assembly, projectProvider, projectLocator);
            var ex = Assert.Throws<ConventionFailedException>(() => Convention.Is(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(), project));

            Approvals.Verify(ex.Message);
        }

        [Test]
        public void assemblies_referencing_bin_obj_with_approved_exceptions()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));


            var projectLocator = Substitute.For<IProjectLocator>();
            var project = new ProjectReferences(typeof(ProjectBasedConventions).Assembly, projectProvider, projectLocator);
            Convention.IsWithApprovedExeptions(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(), project);
        }

        [Test]
        public void scripts_not_embedded_resources()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));

            var projectLocator = Substitute.For<IProjectLocator>();
            var project = new ProjectFileItems(typeof (ProjectBasedConventions).Assembly, projectProvider, projectLocator);
            var ex = Assert.Throws<ConventionFailedException>(() => Convention.Is(new FilesAreEmbeddedResources(".sql"), project));

            Approvals.Verify(ex.Message);
        }

        [Test]
        public void scripts_not_embedded_resources_with_approved_exceptions()
        {
            var projectLocator = Substitute.For<IProjectLocator>();
            var project = new ProjectFileItems(typeof (ProjectBasedConventions).Assembly, projectProvider, projectLocator);
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));

            Convention.IsWithApprovedExeptions(new FilesAreEmbeddedResources(".sql"), project);
        }

        [Test]
        public void release_debug_type_should_be_pdb_only()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithReleaseDebugTypeFull));

            var projectLocator = Substitute.For<IProjectLocator>();
            var propertyGroups = new ProjectPropertyGroups(typeof(ProjectBasedConventions).Assembly, projectProvider, projectLocator);
            var ex =
                Assert.Throws<ConventionFailedException>(
                    () => Convention.Is(new ConfigurationHasSpecificValue(ConfigurationType.Release, "DebugType", "pdbonly"), propertyGroups));

            Approvals.Verify(ex.Message);
        }
        
        [Test]
        public void all_configuration_groups_should_have_platform_AnyCPU()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithReleaseDebugTypeFull));

            var projectLocator = Substitute.For<IProjectLocator>();
            var propertyGroups = new ProjectPropertyGroups(typeof(ProjectBasedConventions).Assembly, projectProvider, projectLocator);
            var ex =
                Assert.Throws<ConventionFailedException>(
                    () => Convention.Is(new ConfigurationHasSpecificValue(ConfigurationType.All, "Platform", "AnyCPU"), propertyGroups));

            Approvals.Verify(ex.Message);
        }
        
        [Test]
        public void all_configuration_groups_should_have_optimize_true_if_property_defined()
        {
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithReleaseDebugTypeFull));

            var projectLocator = Substitute.For<IProjectLocator>();
            var propertyGroups = new ProjectPropertyGroups(typeof(ProjectBasedConventions).Assembly, projectProvider, projectLocator);
            var ex =
                Assert.Throws<ConventionFailedException>(
                    () => Convention.Is(new ConfigurationHasSpecificValue(ConfigurationType.All, "Optimize", "true"), propertyGroups));

            Approvals.Verify(ex.Message);
        }
    }
}