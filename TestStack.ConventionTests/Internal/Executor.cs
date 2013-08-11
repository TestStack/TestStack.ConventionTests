namespace TestStack.ConventionTests.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public static class Executor
    {
        public static ResultInfo GetConventionReport(string conventionTitle, IConventionData data, IEnumerable<object> items)
        {
            var conventionResult = new ResultInfo(
                items.None() ? TestResult.Passed : TestResult.Failed,
                conventionTitle,
                data.Description,
                items.Select(FormatData).ToArray());
            return conventionResult;
        }

        public static ResultInfo GetConventionReportWithApprovedExeptions(string conventionTitle, IConventionData data, IEnumerable<object> items)
        {
            var conventionResult = GetConventionReport(conventionTitle, data, items);
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            // Add approved exceptions to report
            conventionReportTextRenderer.RenderItems(conventionResult);
            conventionResult.WithApprovedException(conventionReportTextRenderer.Output);

            return conventionResult;
        }

        static ConventionReportFailure FormatData<T>(T failingData)
        {
            var formatter = Convention.Formatters.FirstOrDefault(f => f.CanFormat(failingData));

            if (formatter == null)
                throw new NoDataFormatterFoundException(typeof (T).Name +
                                                        " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");

            return formatter.Format(failingData);
        }
    }
}