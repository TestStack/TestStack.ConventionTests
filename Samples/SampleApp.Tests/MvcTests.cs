namespace SampleApp.Tests
{
    using NUnit.Framework;
    using SampleApp.Mvc;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class MvcTests
    {
        [Test]
        public void MvcControllerTest()
        {
            Convention.Is(new MvcControllerNameAndBaseClassConvention(), Types.InAssemblyOf<TestController>());
        } 
    }
}