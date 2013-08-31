namespace TestStack.ConventionTests.Reporting
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public abstract class GroupedByDataTypeRendererBase : AggregatedRenderer
    {
        protected override void Process(IConventionFormatContext context)
        {
            Process(context, AggregatedReports.OrderBy(c => c.ConventionTitle).GroupBy(r => r.DataDescription));
        }

        protected abstract void Process(IConventionFormatContext context, IEnumerable<IGrouping<string, ConventionResult>> resultsGroupedByDataType);
    }
}