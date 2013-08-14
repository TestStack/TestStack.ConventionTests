namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public class ConventionResult : IConventionResult
    {
        readonly List<ResultInfo> conventionResults;
        readonly string dataDescription;

        public ConventionResult(string dataDescription)
        {
            this.dataDescription = dataDescription;
            conventionResults = new List<ResultInfo>();
        }

        public ResultInfo[] ConventionResults
        {
            get { return conventionResults.ToArray(); }
        }

        public void Is<T>(string resultTitle, IEnumerable<T> failingData)
        {
            // ReSharper disable PossibleMultipleEnumeration
            conventionResults.Add(new ResultInfo(
                failingData.None() ? TestResult.Passed : TestResult.Failed,
                resultTitle,
                dataDescription,
                failingData.Select(FormatData).ToArray()));
        }

        public void IsSymmetric<TResult>(
            string firstSetFailureTitle, IEnumerable<TResult> firstSetFailureData,
            string secondSetFailureTitle, IEnumerable<TResult> secondSetFailureData)
        {
            conventionResults.Add(new ResultInfo(
                firstSetFailureData.None() ? TestResult.Passed : TestResult.Failed,
                firstSetFailureTitle,
                dataDescription,
                firstSetFailureData.Select(FormatData).ToArray()));
            conventionResults.Add(new ResultInfo(
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
            var firstSetFailingData = allData.Where(isPartOfFirstSet).Where(d => !isPartOfSecondSet(d));
            var secondSetFailingData = allData.Where(d => !isPartOfFirstSet(d)).Where(isPartOfSecondSet);

            IsSymmetric(
                firstSetFailureTitle, firstSetFailingData,
                secondSetFailureTitle, secondSetFailingData);
        }

        static ConventionReportFailure FormatData<T>(T failingData)
        {
            var formatter = Convention.Formatters.FirstOrDefault(f => f.CanFormat(failingData));

            if (formatter == null)
            {
                throw new NoDataFormatterFoundException(
                    typeof (T).Name +
                    " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");
            }

            return formatter.Format(failingData);
        }
    }
}