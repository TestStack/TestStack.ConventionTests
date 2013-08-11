namespace TestStack.ConventionTests.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public static class Executor
    {
        public static ResultInfo GetConventionReport(string conventionTitle, IConventionContext context,
            IEnumerable<object> items)
        {
            var conventionResult = new ResultInfo(
                items.None() ? TestResult.Passed : TestResult.Failed,
                conventionTitle,
                context.Data.Description,
                items.Select(o => FormatData(o, context)).ToArray());
            return conventionResult;
        }

        public static ResultInfo GetConventionReportWithApprovedExeptions(string conventionTitle,
            IConventionContext context, IEnumerable<object> items)
        {
            var conventionResult = GetConventionReport(conventionTitle, context, items);
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            // Add approved exceptions to report
            conventionReportTextRenderer.RenderItems(conventionResult);
            conventionResult.WithApprovedException(conventionReportTextRenderer.Output);

            return conventionResult;
        }

        static ConventionReportFailure FormatData<T>(T failingData, IConventionContext context)
        {
            var formatter = context.Formatters.FirstOrDefault(f => f.CanFormat(failingData));

            if (formatter == null)
                throw new NoDataFormatterFoundException(typeof (T).Name +
                                                        " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");

            return formatter.Format(failingData);
        }
    }
}