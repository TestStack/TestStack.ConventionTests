namespace TestStack.ConventionTests.Tests
{
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using NUnit.Framework;
    using TestAssembly;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    public class TypeBasedConventions
    {
        readonly Types nhibernateEntities;

        public TypeBasedConventions()
        {
            var itemsToVerify = typeof (SampleDomainClass).Assembly.GetTypes();
            nhibernateEntities = new Types
            {
                ApplicableTypes = itemsToVerify
            };
        }

        [Test]
        public void all_classes_have_default_constructor()
        {
            var ex = Assert.Throws<ConventionFailedException>(()=>Convention.Is(new AllClassesHaveDefaultConstructor(), nhibernateEntities));

            Approvals.Verify(ex.Message);
        }

        [Test]
        public void all_classes_have_default_constructor_wth_approved_exceptions()
        {
            Convention.IsWithApprovedExeptions(new AllClassesHaveDefaultConstructor(), nhibernateEntities);
        }

        [Test]
        public void all_methods_are_virtual()
        {
            var ex = Assert.Throws<ConventionFailedException>(()=>Convention.Is(new AllMethodsAreVirtual(), nhibernateEntities));

            Approvals.Verify(ex.Message);
        }

        [Test]
        public void all_methods_are_virtual_wth_approved_exceptions()
        {
            Convention.IsWithApprovedExeptions(new AllMethodsAreVirtual(), nhibernateEntities);
        }
    }
}