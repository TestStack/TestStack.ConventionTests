namespace TestStack.ConventionTests.Tests
{
    using ApprovalTests.Reporters;
    using NUnit.Framework;
    using TestStack.ConventionTests.Internal;

    [TestFixture]
    [UseReporter(typeof(DiffReporter))] //NOTE: Can we take care of this in IsWithApprovedExceptions?
    public class ConventionAssertionClassTests
    {
        [Test]
        public void approval_mismatch()
        {
            //NOTE Do not approve any changes to this test
            var ex = Assert.Throws<ConventionFailedException>(() => Convention.IsWithApprovedExeptions(new FailingConvention(), new FakeData()));

            StringAssert.Contains("Approved exceptions for convention differs", ex.Message);
            StringAssert.Contains("Failed Approval: Received file ", ex.Message);
            StringAssert.Contains("does not match approved file", ex.Message);
        }

        public class FakeData : IConventionData
        {
            public string Description { get { return "Fake data"; } }

            public bool HasData { get { return true; } }

            public ConventionReportFailure Format(string failingData)
            {
                return new ConventionReportFailure(failingData);
            }
        }

        public class FailingConvention : IConvention<FakeData>
        {
            public void Execute(FakeData data, IConventionResultContext result)
            {
                result.Is("Header", new[] {"Different"});
            }
        }
    }
}