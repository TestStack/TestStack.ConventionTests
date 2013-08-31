namespace SampleApp.Tests
{
    using NUnit.Framework;
    using SampleApp.WebApi;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class WebApiTests
    {
        [Test]
        public void WebApiConventions()
        {
            Convention.Is(new ApiControllerNamingAndBaseClassConvention(), Types.InAssemblyOf<TestApiController>());
        }
    }
}