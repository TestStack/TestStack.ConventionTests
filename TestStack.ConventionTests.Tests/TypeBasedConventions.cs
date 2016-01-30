namespace TestStack.ConventionTests.Tests
{
    using NUnit.Framework;
    using Shouldly;
    using TestAssembly;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class TypeBasedConventions
    {
        readonly Types nhibernateEntities;

        public TypeBasedConventions()
        {
            nhibernateEntities = Types.InAssemblyOf<SampleDomainClass>("nHibernate Entitites",
                type => type.IsConcreteClass() && type.Namespace == typeof (SampleDomainClass).Namespace);
        }

        [Test]
        public void all_classes_have_default_constructor()
        {
            var failures = Convention.GetFailures(new AllClassesHaveDefaultConstructor(), nhibernateEntities);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void all_classes_have_default_constructor_wth_approved_exceptions()
        {
            var failures = Convention.GetFailures(new AllClassesHaveDefaultConstructor(), nhibernateEntities);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void all_methods_are_virtual()
        {
            var failures = Convention.GetFailures(new AllMethodsAreVirtual(), nhibernateEntities);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void all_methods_are_virtual_wth_approved_exceptions()
        {
            var failures = Convention.GetFailures(new AllMethodsAreVirtual(), nhibernateEntities);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void dtos_exists_in_dto_namespace()
        {
            var types = Types.InAssemblyOf<SomeDto>();
            var convention = new ClassTypeHasSpecificNamespace(t => t.Name.EndsWith("Dto"), "TestAssembly.Dtos", "Dto");

            var failures = Convention.GetFailures(convention, types);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void dtos_exists_in_dto_namespace_wth_approved_exceptions()
        {
            var types = Types.InAssemblyOf<SomeDto>();
            var convention = new ClassTypeHasSpecificNamespace(t => t.Name.EndsWith("Dto"), "TestAssembly.Dtos", "Dto");

            Convention.GetFailures(convention, types);
        }
    }
}
