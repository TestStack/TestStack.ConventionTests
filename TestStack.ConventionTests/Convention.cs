namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Reporting;

    public static class Convention
    {
        static IResultsProcessor[] defaultProcessors;
        static IResultsProcessor[] defaultApprovalProcessors;

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
            if (defaultProcessors == null || defaultApprovalProcessors == null)
                Init(Assembly.GetCallingAssembly());
            Execute(convention, data, defaultProcessors, resultProcessor ?? new ConventionReportTextRenderer());
        }

        public static void IsWithApprovedExeptions<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            ITestResultProcessor resultProcessor = null)
            where TDataSource : IConventionData
        {
            if (defaultProcessors == null || defaultApprovalProcessors == null)
                Init(Assembly.GetCallingAssembly());
            Execute(convention, data, defaultApprovalProcessors, resultProcessor ?? new ConventionReportTextRenderer());
        }

        static void Execute<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IResultsProcessor[] processors, ITestResultProcessor resultProcessor)
            where TDataSource : IConventionData
        {
            var dataDescription = string.Format("{0} in {1}", data.GetType().GetSentenceCaseName(), data.Description);
            var context = new ConventionContext(dataDescription, Formatters, processors, resultProcessor);
            context.Execute(convention, data);
        }

        static void Init(Assembly assembly)
        {
            var customReporters = assembly.GetCustomAttributes(typeof(ConventionReporterAttribute), false);

            defaultProcessors = new IResultsProcessor[customReporters.Length + 2];
            defaultApprovalProcessors = new IResultsProcessor[customReporters.Length + 2];

            for (var index = 0; index < customReporters.Length; index++)
            {
                var customReporter = (ConventionReporterAttribute)customReporters[index];
                var resultsProcessor = (IResultsProcessor)Activator.CreateInstance(customReporter.ReporterType);
                defaultProcessors[index] = resultsProcessor;
                defaultApprovalProcessors[index] = resultsProcessor;
            }

            var conventionReportTraceRenderer = new ConventionReportTraceRenderer();
            defaultProcessors[defaultProcessors.Length - 2] = conventionReportTraceRenderer;
            defaultApprovalProcessors[defaultProcessors.Length - 2] = conventionReportTraceRenderer;
            defaultProcessors[defaultProcessors.Length - 1] = new ThrowOnFailureResultsProcessor();
            defaultApprovalProcessors[defaultProcessors.Length - 1] = new ApproveResultsProcessor();
        }
    }
}