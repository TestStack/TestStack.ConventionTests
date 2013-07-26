namespace TestStack.ConventionTests.Tests
{
    using System.Xml.Linq;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using NSubstitute;
    using NUnit.Framework;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Helpers;
    using TestStack.ConventionTests.Tests.Properties;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class ProjectBasedConventions
    {
        [Test]
        public void ReferencingBinObj()
        {
            var projectProvider = Substitute.For<IProjectProvider>();
            var projectLocator = Substitute.For<IProjectLocator>();
            projectProvider
                .LoadProjectDocument(Arg.Any<string>())
                .Returns(XDocument.Parse(Resources.ProjectFileWithBinReference));

            var convention = new ProjectDoesNotReferenceDllsFromBinOrObjDirectories(projectLocator, projectProvider);

            var exception = Assert.Throws<ConventionFailedException>(() =>
                Convention.Is(convention, new[] { typeof(ProjectBasedConventions).Assembly }));
            Approvals.Verify(exception.Message);
        }
    }
}