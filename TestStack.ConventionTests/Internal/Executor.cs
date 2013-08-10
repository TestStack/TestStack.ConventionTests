namespace TestStack.ConventionTests.Internal
{
    using System.Linq;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Reporting;

    public static class Executor
    {
        public static ResultInfo GetConventionReport(string conventionTitle, object[] failingData, IConventionData data)
        {
            if (!data.HasData)
                throw new ConventionSourceInvalidException(string.Format("{0} has no data", data.Description));

            var passed = failingData.None();

            var conventionResult = new ResultInfo(
                passed ? TestResult.Passed : TestResult.Failed,
                conventionTitle,
                data.Description,
                failingData.Select(FormatData).ToArray());
            return conventionResult;
        }

        public static ResultInfo GetConventionReportWithApprovedExeptions(string conventionTitle, object[] failingData, IConventionData data)
        {
            var conventionResult = Executor.GetConventionReport(conventionTitle, failingData, data);
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
                throw new NoDataFormatterFoundException(typeof(T).Name + " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");

            return formatter.Format(failingData);
        }
    }
}