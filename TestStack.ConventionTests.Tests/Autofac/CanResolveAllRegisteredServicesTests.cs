namespace TestStack.ConventionTests.Tests.Autofac
{
    using global::Autofac;
    using NUnit.Framework;
    using TestStack.ConventionTests.Autofac;
    using TestStack.ConventionTests.Tests.Autofac.TestTypes;

    [TestFixture]
    public class CanResolveAllRegisteredServicesTests
    {
        [Test]
        public void ConventionFailsWhenContainerRegistrationCannotBeResolved()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Foo>().As<IFoo>();
            var container = containerBuilder.Build();

            var data = new AutofacRegistrations(container.ComponentRegistry);

            Assert.Throws<ConventionFailedException>(()=>Convention.Is(new CanResolveAllRegisteredServices(container), data));
        }
    }
}