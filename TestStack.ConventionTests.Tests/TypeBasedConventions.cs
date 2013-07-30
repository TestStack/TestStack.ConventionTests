namespace TestStack.ConventionTests.Tests
{
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
            // TODO: This should go to some sort of autodiscovery mechanism so users don't have to see this shit
            Convention.Settings.AssertInclunclusive = Assert.Inconclusive;
            Convention.Settings.AssertZero = (v, m) => Assert.AreEqual(0, v, m);

            var itemsToVerify = typeof (SampleDomainClass).Assembly.GetTypes();
            nhibernateEntities = new Types
            {
                ApplicableTypes = itemsToVerify,
                HasApprovedExceptions = false
            };
        }

        [Test]
        public void all_classes_have_default_constructor()
        {
            Convention.Is(new AllClassesHaveDefaultConstructor(), nhibernateEntities);
        }

        [Test]
        public void all_methods_are_virtual()
        {
            Convention.Is(new AllMethodsAreVirtual(), nhibernateEntities);
        }
    }
}