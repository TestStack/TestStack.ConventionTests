namespace SampleApp.Tests
{
    using System.IO;
    using System.Reflection;
    using NUnit.Framework;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class ProjectConfigurationTests
    {
        readonly string projectLocation;

        public ProjectConfigurationTests()
        {
            projectLocation = Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location,
                @"..\..\..\..\SampleApp\SampleApp.csproj"));
        }

        [Test]
        public void debug_configurations_should_have_debug_type_pdb_only()
        {
            var configurationHasSpecificValue =
                new ConfigurationHasSpecificValue(ConfigurationType.Debug, "DebugType", "full");
            var projectPropertyGroups = new ProjectPropertyGroups(projectLocation);
            Convention.Is(configurationHasSpecificValue, projectPropertyGroups);
        }

        [Test]
        public void debug_configurations_should_have_optimize_false()
        {
            var configurationHasSpecificValue =
                new ConfigurationHasSpecificValue(ConfigurationType.Debug, "Optimize", "false");
            var projectPropertyGroups = new ProjectPropertyGroups(projectLocation);
            Convention.Is(configurationHasSpecificValue, projectPropertyGroups);
        }

        [Test]
        public void release_configurations_should_have_debug_type_pdb_only()
        {
            var configurationHasSpecificValue =
                new ConfigurationHasSpecificValue(ConfigurationType.Release, "DebugType", "pdbonly");
            var projectPropertyGroups = new ProjectPropertyGroups(projectLocation);
            Convention.Is(configurationHasSpecificValue, projectPropertyGroups);
        }

        [Test]
        public void release_configurations_should_have_optimize_true()
        {
            var configurationHasSpecificValue =
                new ConfigurationHasSpecificValue(ConfigurationType.Release, "Optimize", "true");
            var projectPropertyGroups = new ProjectPropertyGroups(projectLocation);
            Convention.Is(configurationHasSpecificValue, projectPropertyGroups);
        }
    }
}