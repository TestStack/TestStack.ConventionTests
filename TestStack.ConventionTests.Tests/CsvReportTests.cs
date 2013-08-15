namespace TestStack.ConventionTests.Tests
{
    using System.Linq;
    using ApprovalTests.Reporters;
    using NUnit.Framework;
    using TestAssembly.Collections;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Tests.TestConventions;

    [UseReporter(typeof(DiffReporter))]
    public class CsvReportTests
    {
        [Test]
        [Explicit("This is work in progress so ignoring for now")]
        public void Can_run_convention_with_simple_reporter()
        {
            Convention.IsWithApprovedExeptions(new CollectionsRelationsConvention(), new Types("Entities")
            {
                TypesToVerify =
                    typeof (Leaf).Assembly.GetExportedTypes()
                        .Where(t => t.Namespace == typeof (Leaf).Namespace).ToArray()
            });
        }
    }
}