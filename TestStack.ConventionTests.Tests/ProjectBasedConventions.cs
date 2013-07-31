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
    [UseReporter(typeof (DiffReporter))]
    public class ProjectBasedConventions
    {
        public ProjectBasedConventions()
        {
            Convention.Settings.AssertInclunclusive = Assert.Inconclusive;
            Convention.Settings.AssertZero = (v, m) => Assert.AreEqual(0, v, m);
        }

        [Test]
        public void ReferencingBinObj()
        {
            var projectProvider = Substitute.For<IProjectProvider>();
            var projectLocator = Substitute.For<IProjectLocator>();
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            Convention.Is(new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(),
                new Project(typeof (ProjectBasedConventions).Assembly, projectProvider, projectLocator));
        }
         
        [Test]
        public void ScriptsNotEmbeddedResources()
        {
            var projectProvider = Substitute.For<IProjectProvider>();
            var projectLocator = Substitute.For<IProjectLocator>();
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithInvalidSqlScriptFile));

            Convention.Is(new FilesAreEmbeddedResources(),
                new Project(typeof (ProjectBasedConventions).Assembly, projectProvider, projectLocator)
                {
                    Includes = i => i.EndsWith(".sql")
                });
        }
    }
}