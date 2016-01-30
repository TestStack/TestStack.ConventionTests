namespace TestStack.ConventionTests.Tests
{
    using System.Xml.Linq;
    using NSubstitute;
    using NUnit.Framework;
    using Shouldly;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Tests.Properties;

    [TestFixture]
    public class ProjectBasedConventions
    {
        IProjectProvider projectProvider;

        [SetUp]
        public void Setup()
        {
            projectProvider = Substitute.For<IProjectProvider>();
            projectProvider.GetName().Returns("ProjectName");
        }

        [Test]
        public void assemblies_referencing_bin_obj()
        {
            projectProvider
                .LoadProjectDocument()
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));
            
            var project = new ProjectReferences(projectProvider);
            var failures = Convention.GetFailures(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(), project);
            
            failures.ShouldMatchApproved();
        }

        [Test]
        public void assemblies_referencing_bin_obj_with_approved_exceptions()
        {
            projectProvider
                .LoadProjectDocument()
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            var project = new ProjectReferences(projectProvider);
            var failures = Convention.GetFailures(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(), project);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void scripts_not_embedded_resources()
        {
            projectProvider
                .LoadProjectDocument()
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));
            
            var project = new ProjectFileItems(projectProvider);
            var failures = Convention.GetFailures(new FilesAreEmbeddedResources(".sql"), project);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void scripts_not_embedded_resources_with_approved_exceptions()
        {
            var project = new ProjectFileItems(projectProvider);
            projectProvider
                .LoadProjectDocument()
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));

            Convention.GetFailures(new FilesAreEmbeddedResources(".sql"), project);
        }

        [Test]
        public void release_debug_type_should_be_pdb_only()
        {
            projectProvider
                .LoadProjectDocument()
                .Returns(XDocument.Parse(Resources.ProjectFileWithReleaseDebugTypeFull));

            var propertyGroups = new ProjectPropertyGroups(projectProvider);
            var failures = Convention.GetFailures(new ConfigurationHasSpecificValue(ConfigurationType.Release, "DebugType", "pdbonly"), propertyGroups);

            failures.ShouldMatchApproved();
        }
        
        [Test]
        public void all_configuration_groups_should_have_platform_AnyCPU()
        {
            projectProvider
                .LoadProjectDocument()
                .Returns(XDocument.Parse(Resources.ProjectFileWithReleaseDebugTypeFull));

            var propertyGroups = new ProjectPropertyGroups(projectProvider);
            var failures = Convention.GetFailures(new ConfigurationHasSpecificValue(ConfigurationType.All, "Platform", "AnyCPU"), propertyGroups);

            failures.ShouldMatchApproved();
        }
        
        [Test]
        public void all_configuration_groups_should_have_optimize_true_if_property_defined()
        {
            projectProvider
                .LoadProjectDocument()
                .Returns(XDocument.Parse(Resources.ProjectFileWithReleaseDebugTypeFull));

            var propertyGroups = new ProjectPropertyGroups(projectProvider);
            var failures =
                Convention.GetFailures(new ConfigurationHasSpecificValue(ConfigurationType.All, "Optimize", "true"),
                    propertyGroups);

            failures.ShouldMatchApproved();
        }
    }
}