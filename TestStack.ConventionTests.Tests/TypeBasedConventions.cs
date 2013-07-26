namespace TestStack.ConventionTests.Tests
{
    using System;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using NUnit.Framework;
    using TestAssembly;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class TypeBasedConventions
    {
        readonly Type[] itemsToVerify;

        public TypeBasedConventions()
        {
            itemsToVerify = typeof(SampleDomainClass).Assembly.GetTypes();
        }

        [Test]
        public void all_methods_are_virtual()
        {
            var exception = Assert.Throws<ConventionFailedException>(() => Convention.Is<AllMethodsAreVirtual>(itemsToVerify));

            Approvals.Verify(exception.Message);
        }

        [Test]
        public void all_classes_have_default_constructor()
        {
            var exception = Assert.Throws<ConventionFailedException>(() => Convention.Is<AllClassesHaveDefaultConstructor>(itemsToVerify));

            Approvals.Verify(exception.Message);
        }
    }
}