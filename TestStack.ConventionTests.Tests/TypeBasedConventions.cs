namespace TestStack.ConventionTests.Tests
{
    using System.Linq;
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using NUnit.Framework;
    using TestAssembly;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    public class TypeBasedConventions
    {
        readonly Types nhibernateEntities;

        public TypeBasedConventions()
        {
            nhibernateEntities = Types.InAssemblyOf<SampleDomainClass>("nHibernate Entitites",
                types => types.Where(t => t.IsConcreteClass() && t.Namespace == typeof (SampleDomainClass).Namespace));
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

        [Test]
        public void dtos_exists_in_dto_namespace()
        {
            var types = Types.InAssemblyOf<SomeDto>();
            var convention = new ClassTypeHasSpecificNamespace(t => t.Name.EndsWith("Dto"), "TestAssembly.Dtos", "Dto");

            var ex = Assert.Throws<ConventionFailedException>(() =>Convention.Is(convention, types));
            Approvals.Verify(ex.Message);
        }

        [Test]
        public void dtos_exists_in_dto_namespace_wth_approved_exceptions()
        {
            var types = Types.InAssemblyOf<SomeDto>();
            var convention = new ClassTypeHasSpecificNamespace(t => t.Name.EndsWith("Dto"), "TestAssembly.Dtos", "Dto");

            Convention.IsWithApprovedExeptions(convention, types);
        }
    }
}
