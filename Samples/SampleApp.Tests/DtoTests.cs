namespace SampleApp.Tests
{
    using NUnit.Framework;
    using SampleApp.Dtos;
    using TestStack.ConventionTests;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Conventions;

    [TestFixture]
    public class DtoTests
    {
        [Test]
        public void DtosMustOnlyExistInDtoNameSpace()
        {
            Convention.Is(new ClassTypeHasSpecificNamespace(t=>t.Name.EndsWith("Dto"), "SampleApp.Dtos", "Dto"),
                Types.InAssemblyOf<SomeDto>());
        }
    }
}