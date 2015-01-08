namespace SampleApp.Tests
{
    using NUnit.Framework;
    using SampleApp.Domain;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class DomainTests
    {
        readonly Types domainEntities;

        public DomainTests()
        {
            domainEntities = Types.InAssemblyOf<DomainClass>("Domain Entities", 
                type => type.Namespace.StartsWith("SampleApp.Domain"));
        }

        [Test]
        public void DomainClassesShouldHaveDefaultConstructor()
        {
            Convention.Is(new AllClassesHaveDefaultConstructor(), domainEntities);
        }

        [Test]
        public void DomainClassesShouldHaveVirtualProperties()
        {
            Convention.Is(new AllMethodsAreVirtual(), domainEntities);
        }
    }
}