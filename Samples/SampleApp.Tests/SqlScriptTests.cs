namespace SampleApp.Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using NUnit.Framework;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class SqlScriptTests
    {
        readonly string projectLocation;

        public SqlScriptTests()
        {
            projectLocation =
                Path.GetFullPath(Path.Combine(Assembly.GetExecutingAssembly().Location,
                    @"..\..\..\..\SampleApp\SampleApp.csproj"));
        }

        [Test]
        public void SqlScriptsShouldBeEmbeddedResources()
        {
            Convention.Is(new FilesAreEmbeddedResources(".sql"), new ProjectFileItems(projectLocation));
        }
    }
}