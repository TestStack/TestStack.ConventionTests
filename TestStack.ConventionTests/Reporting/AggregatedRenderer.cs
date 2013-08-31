namespace TestStack.ConventionTests.Reporting
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    /// <summary>
    /// Aggregates all previous results
    /// </summary>
    public abstract class AggregatedRenderer : IResultsProcessor
    {
        static readonly List<ConventionResult> Reports = new List<ConventionResult>();
        public IEnumerable<ConventionResult> AggregatedReports { get { return Reports; } }

        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            Reports.AddRange(results.Except(Reports));
            Process(context);
        }

        protected abstract void Process(IConventionFormatContext context);
    }
}