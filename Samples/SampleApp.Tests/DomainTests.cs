namespace SampleApp.Tests
{
    using System.Linq;
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
                types => types.Where(t=>t.Namespace.StartsWith("SampleApp.Domain")));
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