namespace TestStack.ConventionTests.Tests
{
    using ApprovalTests;
    using ApprovalTests.Reporters;
    using NUnit.Framework;
    using TestAssembly.Controllers;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))] 
    public class MvcConventions
    {
        [Test]
        public void controller_conventions()
        {
            var types = Types.InAssemblyOf<TestController>();
            var convention = new MvcControllerNameAndBaseClassConvention();

            var ex = Assert.Throws<ConventionFailedException>(() => Convention.Is(convention, types));
            Approvals.Verify(ex.Message);
        }

        [Test]
        public void api_controller_conventions()
        {
            var types = Types.InAssemblyOf<TestController>();
            var convention = new ApiControllerNamingAndBaseClassConvention();

            var ex = Assert.Throws<ConventionFailedException>(() => Convention.Is(convention, types));
            Approvals.Verify(ex.Message);
        } 
    }
}