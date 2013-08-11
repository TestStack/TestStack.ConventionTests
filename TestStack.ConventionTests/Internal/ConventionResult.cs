namespace TestStack.ConventionTests.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public class ConventionResult : IConventionResult
    {
        readonly string dataDescription;
        readonly List<ResultInfo> conventionResults;

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

        public void IsSymmetric<T>(
            string firstResultTitle, IEnumerable<T> firstFailingData,
            string secondResultTitle, IEnumerable<T> secondFailingData)
        {
            conventionResults.Add(new ResultInfo(
                firstFailingData.None() ? TestResult.Passed : TestResult.Failed,
                firstResultTitle,
                dataDescription,
                firstFailingData.Select(FormatData).ToArray()));
            conventionResults.Add(new ResultInfo(
                secondFailingData.None() ? TestResult.Passed : TestResult.Failed,
                secondResultTitle,
                dataDescription,
                secondFailingData.Select(FormatData).ToArray()));
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