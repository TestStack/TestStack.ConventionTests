namespace TestStack.ConventionTests.Tests.Autofac
{
    using global::Autofac;
    using NUnit.Framework;
    using TestStack.ConventionTests.Autofac;
    using TestStack.ConventionTests.Tests.Autofac.TestTypes;

    [TestFixture]
    public class ServicesShouldOnlyHaveDependenciesWithLesserLifetimeTests
    {
        [Test]
        public void ConventionShouldFailForTransientInjectectedIntoSingleton()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<Foo>().As<IFoo>().SingleInstance();
            containerBuilder.RegisterType<Bar>().As<IBar>();

            var container = containerBuilder.Build();

            var convention = new ServicesShouldOnlyHaveDependenciesWithLesserLifetime();
            var autofacRegistrations = new AutofacRegistrations(container.ComponentRegistry);
            Assert.Throws<ConventionFailedException>(() => Convention.Is(convention, autofacRegistrations));
        }
    }
}