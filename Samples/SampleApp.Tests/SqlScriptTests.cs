namespace SampleApp.Tests
{
    using NUnit.Framework;
    using SampleApp.Domain;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class SqlScriptTests
    {
        [Test]
        public void SqlScriptsShouldBeEmbeddedResources()
        {
            Convention.Is(new FilesAreEmbeddedResources(".sql"), new ProjectFileItems(typeof(DomainClass).Assembly));
        }
    }
}
