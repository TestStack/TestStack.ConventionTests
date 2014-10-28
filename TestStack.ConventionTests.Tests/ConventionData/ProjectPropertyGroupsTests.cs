namespace TestStack.ConventionTests.Tests.ConventionData
{
    using System.Linq;
    using System.Xml.Linq;
    using NSubstitute;
    using NUnit.Framework;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Tests.Properties;

    [TestFixture]
    public class ProjectPropertyGroupsTests
    {
        IProjectProvider projectProvider;

        [SetUp]
        public void Setup()
        {
            projectProvider = Substitute.For<IProjectProvider>();
        }

        [Test]
        public void can_parse_a_normal_project_file_to_read_global_debug_and_release_property_groups()
        {
            projectProvider
                   .LoadProjectDocument(Arg.Any<string>())
                   .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            var projectGroups = new ProjectPropertyGroups(typeof(ProjectPropertyGroups).Assembly, projectProvider, Substitute.For<IProjectLocator>());
            
            Assert.That(projectGroups.PropertyGroups.Length, Is.EqualTo(3));
            Assert.That(projectGroups.PropertyGroups.Any(item => item.Debug));
            Assert.That(projectGroups.PropertyGroups.Any(item => item.Release));
            Assert.That(projectGroups.PropertyGroups.Any(item => item.Global));
        }
    }
}