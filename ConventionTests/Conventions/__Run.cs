using ApprovalTests.Reporters;

[assembly: UseReporter(typeof (DiffReporter))]

namespace ConventionTests
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using NUnit.Framework;

    [TestFixture]
    public class ConventionTestsRunner
    {
        [TestFixtureSetUp]
        public static void GlobalSetUp()
        {
        }

        public TestCaseData[] Conventions
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                var conventions = ReflectionExtensions.GetAllConventions(Assembly.GetExecutingAssembly());
                var tests = Array.ConvertAll(conventions, BuildTestData);
                return tests;
            }
        }

        TestCaseData BuildTestData(IConventionTest convention)
        {
            return new TestCaseData(convention).SetName(convention.Name);
        }

        class NUnitAssert : IAssert
        {
            public void Inconclusive(string message)
            {
                Assert.Inconclusive(message);
            }

            public void AreEqual(int expected, int actual, string message)
            {
                Assert.AreEqual(expected, actual, message);
            }
        }

        [Test]
        [TestCaseSource("Conventions")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Run(IConventionTest test)
        {
            test.Execute(new NUnitAssert());
        }
    }
}