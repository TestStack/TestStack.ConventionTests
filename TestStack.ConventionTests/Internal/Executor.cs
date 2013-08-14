namespace TestStack.ConventionTests.Internal
{
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Reporting;

    public static class Executor
    {
        public static ResultInfo[] GetConventionResults<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            if (!data.HasData)
                throw new ConventionSourceInvalidException(string.Format("{0} has no data", data.Description));

            var resultGatherer = new ConventionResult(data.Description, Convention.Formatters);
            convention.Execute(data, resultGatherer);

            return resultGatherer.ConventionResults;
        }

        public static ResultInfo[] GetConventionResultsWithApprovedExeptions<TDataSource>(
            IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            // Add approved exceptions to report
            if (!data.HasData)
                throw new ConventionSourceInvalidException(string.Format("{0} has no data", data.Description));

            var resultGatherer = new ConventionResult(data.Description, Convention.Formatters);
            convention.Execute(data, resultGatherer);
            foreach (var conventionResult in resultGatherer.ConventionResults)
            {
                conventionReportTextRenderer.RenderItems(conventionResult);
                conventionResult.WithApprovedException(conventionReportTextRenderer.Output);
            }

            return resultGatherer.ConventionResults;
        }
    }
}