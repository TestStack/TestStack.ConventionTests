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
        readonly IList<ConventionResult> results = new List<ConventionResult>();

        public ConventionContext(string dataDescription, IList<IReportDataFormatter> formatters,
            IList<IResultsProcessor> processors)
        {
            this.formatters = formatters;
            this.processors = processors;
            this.dataDescription = dataDescription;
        }

        public ConventionResult[] ConventionResults
        {
            get { return results.ToArray(); }
        }

        ConventionReportFailure IConventionFormatContext.FormatData(object failingData)
        {
            var formatter = formatters.FirstOrDefault(f => f.CanFormat(failingData));
            if (formatter == null)
            {
                throw new NoDataFormatterFoundException(
                    failingData.GetType().Name +
                    " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");
            }

            return formatter.Format(failingData);
        }

        void IConventionResultContext.Is<TResult>(string resultTitle, IEnumerable<TResult> failingData)
        {
            // ReSharper disable PossibleMultipleEnumeration
            results.Add(new ConventionResult(
                typeof(TResult),
                resultTitle,
                dataDescription,
                failingData.ToObjectArray()));
        }

        void IConventionResultContext.IsSymmetric<TResult>(
            string firstSetFailureTitle, IEnumerable<TResult> firstSetFailureData,
            string secondSetFailureTitle, IEnumerable<TResult> secondSetFailureData)
        {
            results.Add(new ConventionResult(
                typeof(TResult), firstSetFailureTitle,
                dataDescription,
                firstSetFailureData.ToObjectArray()));
            results.Add(new ConventionResult(
                typeof(TResult), secondSetFailureTitle,
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
            var firstSetFailingData = allData.Where(isPartOfFirstSet).Unless(isPartOfSecondSet);
            var secondSetFailingData = allData.Where(isPartOfSecondSet).Unless(isPartOfFirstSet);

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

            foreach (var resultsProcessor in processors)
            {
                resultsProcessor.Process(this, ConventionResults);
            }
        }
    }
}