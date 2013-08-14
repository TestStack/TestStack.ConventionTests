namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Reporting;

    public class ConventionContext : IConventionResultContext
    {
        readonly string dataDescription;
        readonly IList<IReportDataFormatter> formatters;
        readonly IList<ConventionResult> results = new List<ConventionResult>();

        public ConventionContext(string dataDescription, IList<IReportDataFormatter> formatters)
        {
            this.formatters = formatters;
            this.dataDescription = dataDescription;
        }

        public ConventionResult[] ConventionResults
        {
            get { return results.ToArray(); }
        }

        public void Is<T>(string resultTitle, IEnumerable<T> failingData)
        {
            // ReSharper disable PossibleMultipleEnumeration
            results.Add(new ConventionResult(
                failingData.None() ? TestResult.Passed : TestResult.Failed,
                resultTitle,
                dataDescription,
                failingData.Select(FormatData).ToArray()));
        }

        public void IsSymmetric<TResult>(
            string firstSetFailureTitle, IEnumerable<TResult> firstSetFailureData,
            string secondSetFailureTitle, IEnumerable<TResult> secondSetFailureData)
        {
            results.Add(new ConventionResult(
                firstSetFailureData.None() ? TestResult.Passed : TestResult.Failed,
                firstSetFailureTitle,
                dataDescription,
                firstSetFailureData.Select(FormatData).ToArray()));
            results.Add(new ConventionResult(
                secondSetFailureData.None() ? TestResult.Passed : TestResult.Failed,
                secondSetFailureTitle,
                dataDescription,
                secondSetFailureData.Select(FormatData).ToArray()));
        }

        public void IsSymmetric<TResult>(
            string firstSetFailureTitle,
            string secondSetFailureTitle,
            Func<TResult, bool> isPartOfFirstSet,
            Func<TResult, bool> isPartOfSecondSet,
            IEnumerable<TResult> allData)
        {
            var firstSetFailingData = allData.Where(isPartOfFirstSet).Unless(isPartOfSecondSet);
            var secondSetFailingData = allData.Where(isPartOfSecondSet).Unless(isPartOfFirstSet);

            IsSymmetric(
                firstSetFailureTitle, firstSetFailingData,
                secondSetFailureTitle, secondSetFailingData);
        }

        ConventionReportFailure FormatData<T>(T failingData)
        {
            var formatter = formatters.FirstOrDefault(f => f.CanFormat(failingData));
            if (formatter == null)
            {
                throw new NoDataFormatterFoundException(
                    typeof (T).Name +
                    " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");
            }

            return formatter.Format(failingData);
        }

        public ConventionResult[] GetConventionResults<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            if (!data.HasData)
                throw new ConventionSourceInvalidException(String.Format("{0} has no data", data.Description));

            convention.Execute(data, this);

            return ConventionResults;
        }

        public ConventionResult[] GetConventionResultsWithApprovedExeptions<TDataSource>(
            IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            // Add approved exceptions to report
            if (!data.HasData)
                throw new ConventionSourceInvalidException(String.Format("{0} has no data", data.Description));

            convention.Execute(data, this);
            foreach (var conventionResult in ConventionResults)
            {
                conventionReportTextRenderer.RenderItems(conventionResult);
                conventionResult.WithApprovedException(conventionReportTextRenderer.Output);
            }

            return ConventionResults;
        }
    }
}