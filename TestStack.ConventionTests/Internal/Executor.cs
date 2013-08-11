namespace TestStack.ConventionTests.Internal
{
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public static class Executor
    {
        public static ResultInfo GetConventionReport(string conventionTitle, ConventionResult failingData,
            IConventionData data)
        {
            var result = failingData.Failed;

            var conventionResult = new ResultInfo(
                result ? TestResult.Passed : TestResult.Failed,
                conventionTitle,
                data.Description,
                failingData.Items.Select(FormatData).ToArray());
            return conventionResult;
        }

        public static ResultInfo GetConventionReportWithApprovedExeptions(string conventionTitle,
            ConventionResult failingData, IConventionData data)
        {
            var conventionResult = GetConventionReport(conventionTitle, failingData, data);
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