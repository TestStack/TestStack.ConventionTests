namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Reporting;

    public class ConventionContext : IConventionResultContext, IConventionFormatContext
    {
        readonly string dataDescription;
        readonly IList<IReportDataFormatter> formatters;
        readonly IList<IResultsProcessor> processors;
        readonly ITestResultProcessor testResultProcessor;
        readonly IList<ConventionResult> results = new List<ConventionResult>();
        bool resultSet;

        public ConventionContext(string dataDescription, IList<IReportDataFormatter> formatters,
            IList<IResultsProcessor> processors, ITestResultProcessor testResultProcessor)
        {
            this.formatters = formatters;
            this.processors = processors;
            this.testResultProcessor = testResultProcessor;
            this.dataDescription = dataDescription;
        }

        public ConventionResult[] ConventionResults
        {
            get { return results.ToArray(); }
        }

        string IConventionFormatContext.FormatDataAsHtml(object data)
        {
            var formatter = GetReportDataFormatterFor(data);
            return formatter.FormatHtml(data);
        }

        ITestResultProcessor IConventionFormatContext.TestResultProcessor
        {
            get { return testResultProcessor; }
        }

        string IConventionFormatContext.FormatDataAsString(object data)
        {
            var formatter = GetReportDataFormatterFor(data);

            return formatter.FormatString(data);
        }

        IReportDataFormatter GetReportDataFormatterFor(object data)
        {
            IReportDataFormatter formatter = formatters.FirstOrDefault(f => f.CanFormat(data));
            if (formatter == null)
            {
                throw new NoDataFormatterFoundException(
                    data.GetType().Name + " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");
            }
            return formatter;
        }

        void IConventionResultContext.Is<TResult>(string resultTitle, IEnumerable<TResult> failingData)
        {
            resultSet = true;
            // ReSharper disable PossibleMultipleEnumeration
            results.Add(new ConventionResult(
                typeof (TResult),
                resultTitle,
                dataDescription,
                failingData.ToObjectArray()));
        }

        void IConventionResultContext.IsSymmetric<TResult>(
            string firstSetFailureTitle, IEnumerable<TResult> firstSetFailureData,
            string secondSetFailureTitle, IEnumerable<TResult> secondSetFailureData)
        {
            resultSet = true;
            results.Add(new ConventionResult(
                typeof (TResult), firstSetFailureTitle,
                dataDescription,
                firstSetFailureData.ToObjectArray()));
            results.Add(new ConventionResult(
                typeof (TResult), secondSetFailureTitle,
                dataDescription,
                secondSetFailureData.ToObjectArray()));
        }

        void IConventionResultContext.IsSymmetric<TResult>(
            string firstSetFailureTitle,
            string secondSetFailureTitle,
            Func<TResult, bool> isPartOfFirstSet,
            Func<TResult, bool> isPartOfSecondSet,
            IEnumerable<TResult> allData)
        {
            IEnumerable<TResult> firstSetFailingData = allData.Where(isPartOfFirstSet).Unless(isPartOfSecondSet);
            IEnumerable<TResult> secondSetFailingData = allData.Where(isPartOfSecondSet).Unless(isPartOfFirstSet);

            (this as IConventionResultContext).IsSymmetric(
                firstSetFailureTitle, firstSetFailingData,
                secondSetFailureTitle, secondSetFailingData);
        }

        public void Execute<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            if (!data.HasData)
                throw new ConventionSourceInvalidException(String.Format("{0} has no data", data.Description));
            convention.Execute(data, this);

            if (!resultSet) 
                throw new ResultNotSetException("{0} did not set a result, conventions must always set a result");

            foreach (IResultsProcessor resultsProcessor in processors)
            {
                resultsProcessor.Process(this, ConventionResults);
            }
        }
    }
}