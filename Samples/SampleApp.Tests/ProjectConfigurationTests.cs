namespace SampleApp.Tests
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using NSubstitute;
    using NUnit.Framework;
    using SampleApp.Domain;
    using SampleApp.Dtos;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;

    [TestFixture]
    public class ProjectConfigurationTests
    {
        IProjectLocator projectLocator;
        IProjectProvider projectProvider;

        [SetUp]
        public void Setup()
        {
            projectLocator = Substitute.For<IProjectLocator>();
            projectProvider = Substitute.For<IProjectProvider>();
            projectProvider
               .LoadProjectDocument(Arg.Any<string>())
               .Returns(XDocument.Parse(File.ReadAllText(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\SampleApp\SampleApp.csproj")))));
        }

        [Test]
        public void debug_configurations_should_have_debug_type_pdb_only()
        {
            Convention.Is(new ConfigurationHasSpecificValue(ConfigurationType.Debug, "DebugType", "full"), new ProjectPropertyGroups(typeof(DomainClass).Assembly, projectProvider, projectLocator));
        }

        [Test]
        public void debug_configurations_should_have_optimize_false()
        {
            Convention.Is(new ConfigurationHasSpecificValue(ConfigurationType.Debug, "Optimize", "false"), new ProjectPropertyGroups(typeof(DomainClass).Assembly, projectProvider, projectLocator));
        }

        [Test]
        public void release_configurations_should_have_debug_type_pdb_only()
        {
            Convention.Is(new ConfigurationHasSpecificValue(ConfigurationType.Release, "DebugType", "pdbonly"), new ProjectPropertyGroups(typeof(DomainClass).Assembly, projectProvider, projectLocator));
        }

        [Test]
        public void release_configurations_should_have_optimize_true()
        {
            Convention.Is(new ConfigurationHasSpecificValue(ConfigurationType.Release, "Optimize", "true"), new ProjectPropertyGroups(typeof(DomainClass).Assembly, projectProvider, projectLocator));
        }
    }
}