namespace TestStack.ConventionTests.Tests
{
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using NUnit.Framework;
    using TestAssembly;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class TypeBasedConventions
    {
        [Test]
        public void Test()
        {
            var types = typeof(ClassWithNonVirtualMethod).Assembly.GetTypes();

            var exception = Assert.Throws<ConventionFailedException>(() => Convention.Is<AllMethodsAreVirtual>(types));

            Approvals.Verify(exception.Message);
        }
    }
}