namespace TestStack.ConventionTests.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using Shouldly;
    using TestAssembly.Collections;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Reporting;
    using TestStack.ConventionTests.Tests.TestConventions;
    
    public class CsvReportTests
    {
        [Test]
        public void Can_run_convention_with_simple_reporter()
        {
            var typesToVerify = typeof (Leaf).Assembly.GetExportedTypes()
                .Where(t => t.Namespace == typeof (Leaf).Namespace);

            var failures = Convention.GetFailures(new CollectionsRelationsConvention(),
                new Types(typesToVerify, "Entities"),
                new CsvReporter());

            failures.ShouldMatchApproved();
        }
    }
}