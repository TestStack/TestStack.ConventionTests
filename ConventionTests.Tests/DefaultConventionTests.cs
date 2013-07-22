namespace ConventionTests.Tests
{
    using System;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using AssemblyUnderTest;
    using NUnit.Framework;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class DefaultConventionTests
    {
        Type[] sourceTypes;

        [SetUp]
        public void Setup()
        {
            sourceTypes = typeof(SampleDomainClass).Assembly.GetTypes();
        }

        [Test]
        public void all_methods_are_virtual()
        {
            var virtualConvention = new AllMethodsAreVirtualConvention(sourceTypes);

            var exception = Assert.Throws<ConventionFailedException>(virtualConvention.AssertConvention);
            Approvals.Verify(exception.Message);
        }

        [Test]
        public void all_classes_have_default_constructor()
        {
            var constructorConvention = new ClassHasDefaultConstructorConvention(sourceTypes);

            var exception = Assert.Throws<ConventionFailedException>(constructorConvention.AssertConvention);
            Approvals.Verify(exception.Message);
        }
    }
}
