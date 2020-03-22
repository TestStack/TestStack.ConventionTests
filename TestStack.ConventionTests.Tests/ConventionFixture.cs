namespace TestStack.ConventionTests.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    [TestFixture]
    public class ConventionFixture
    {
        [Test]
        public void ShouldThrowWhenConventionDoesNotSetResult()
        {
            Assert.Throws<ResultNotSetException>(() => 
                Convention.Is(new CustomConventionWhichDoesNothing(), Types.InAssemblyOf<ConventionFixture>()));
        }

        // ReSharper disable once UnusedVariable
        class CustomConventionWhichDoesNothing : IConvention<Types>
        {
            public void Execute(Types data, IConventionResultContext result)
            {
                var failingTypes = data.TypesToVerify.Where(IsBroken);
                // Oops, I forgot to set the result
            }

            public string ConventionReason => "Convention does not set result for testing";

            bool IsBroken(Type type)
            {
                return true;
            }
        }
    }
}