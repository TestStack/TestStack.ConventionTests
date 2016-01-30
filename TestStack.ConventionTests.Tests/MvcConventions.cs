namespace TestStack.ConventionTests.Tests
{
    using NUnit.Framework;
    using Shouldly;
    using TestAssembly.Controllers;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class MvcConventions
    {
        [Test]
        public void controller_conventions()
        {
            var types = Types.InAssemblyOf<TestController>();
            var convention = new MvcControllerNameAndBaseClassConvention();

            var failures = Convention.GetFailures(convention, types);

            failures.ShouldMatchApproved();
        }

        [Test]
        public void api_controller_conventions()
        {
            var types = Types.InAssemblyOf<TestController>();
            var convention = new ApiControllerNamingAndBaseClassConvention();

            var failures = Convention.GetFailures(convention, types);

            failures.ShouldMatchApproved();
        } 
    }
}