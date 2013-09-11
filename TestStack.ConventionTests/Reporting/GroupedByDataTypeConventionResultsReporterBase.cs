namespace TestStack.ConventionTests.Reporting
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public abstract class GroupedByDataTypeConventionResultsReporterBase : AggregatedConventionResultsReporter
    {
        protected GroupedByDataTypeConventionResultsReporterBase(string outputFilename) : base(outputFilename)
        {
        }

        protected override string Process(IConventionFormatContext context)
        {
            return Process(context, AggregatedReports.OrderBy(c => c.ConventionTitle).GroupBy(r => r.DataDescription));
        }

        protected abstract string Process(IConventionFormatContext context, IEnumerable<IGrouping<string, ConventionResult>> resultsGroupedByDataType);
    }
}