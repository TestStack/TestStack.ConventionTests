namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Reporting;

    public static class Convention
    {
        static IResultsProcessor[] defaultProcessors;

        static Convention()
        {
            Formatters = new List<IReportDataFormatter>
            {
                new TypeDataFormatter(),
                new ProjectReferenceFormatter(),
                new ProjectFileFormatter(),
                new MethodInfoDataFormatter(),
                new StringDataFormatter(),
                new ConvertibleFormatter(),
                new FallbackFormatter()
            };
        }

        public static IList<IReportDataFormatter> Formatters { get; set; }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            ITestResultProcessor resultProcessor = null)
            where TDataSource : IConventionData
        {
            if (defaultProcessors == null) {
                // var assembly = typeof(TDataSource).GetTypeInfo().Assembly;
                var attributes = typeof(TDataSource).GetTypeInfo().GetCustomAttributes(typeof(ConventionReporterAttribute), false);
                Init(attributes.ToList());
                // Init(Assembly.GetCallingAssembly());
            }
                

            Execute(convention, data, defaultProcessors.Concat(new[] { new ThrowOnFailureResultsProcessor() }), resultProcessor ?? new ConventionReportTextRenderer());
        }

        /// <summary>
        /// Verifies a convention, returning the failures as a string instead of throwing an exception
        /// 
        /// Allows you to approve or assert against the failures yourself
        /// </summary>
        /// <example>Convention.GetFailures(new SomeConvention(), Types.InAssemblyOf&lt;SomeTypeOfMine&gt;())</example>
        public static string GetFailures<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            ITestResultProcessor resultProcessor = null)
            where TDataSource : IConventionData
        {
            if (defaultProcessors == null) {

            }
                var attributes = typeof(TDataSource).GetTypeInfo().GetCustomAttributes(typeof(ConventionReporterAttribute), false);
                Init(attributes.ToList());
            var captureFailuresProcessor = new CaptureFailuresProcessor();
            Execute(convention, data, defaultProcessors.Concat(new [] { captureFailuresProcessor }), resultProcessor ?? new ConventionReportTextRenderer());
            return captureFailuresProcessor.Failures;
        }

        static void Execute<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IEnumerable<IResultsProcessor> processors, ITestResultProcessor resultProcessor)
            where TDataSource : IConventionData
        {
            var dataDescription = $"{data.GetType().GetSentenceCaseName()} in {data.Description}";
            var context = new ConventionContext(dataDescription, Formatters, processors, resultProcessor);
            context.Execute(convention, data);
        }

        static void Init(IList<Attribute> attributes)
        {
            var customReporters = attributes;
            defaultProcessors = new IResultsProcessor[customReporters.Count + 1];

            for (var index = 0; index < customReporters.Count; index++)
            {
                var customReporter = (ConventionReporterAttribute)customReporters[index];
                var resultsProcessor = (IResultsProcessor)Activator.CreateInstance(customReporter.ReporterType);
                defaultProcessors[index] = resultsProcessor;
            }

            var conventionReportTraceRenderer = new ConventionReportTraceRenderer();
            defaultProcessors[defaultProcessors.Length - 1] = conventionReportTraceRenderer;
        }
    }
}