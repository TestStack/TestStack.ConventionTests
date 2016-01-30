namespace SampleApp.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using SampleApp.Domain;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class SqlScriptTests
    {
        string projectLocation;

        public SqlScriptTests()
        {
            projectLocation = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\SampleApp\SampleApp.csproj"));
        }

        [Test]
        public void SqlScriptsShouldBeEmbeddedResources()
        {
            Convention.Is(new FilesAreEmbeddedResources(".sql"), new ProjectFileItems(projectLocation));
        }
    }
}
