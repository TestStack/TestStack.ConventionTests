namespace TestStack.ConventionTests
{
    using System.Collections.Generic;
    using System.Linq;
    using ApprovalTests;
    using ApprovalTests.Core.Exceptions;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;

    public static class Convention
    {
        static readonly List<ConventionReport> Reports = new List<ConventionReport>();

        public static IEnumerable<ConventionReport> ConventionReports { get { return Reports; } }

        public static void Is<TDataSource, TDataType>(IConvention<TDataSource, TDataType> convention, TDataSource data)
            where TDataSource : IConventionData, ICreateReportLineFor<TDataType>
        {
            Is(convention, data, new ConventionResultExceptionReporter());
        }

        public static void Is<TDataSource, TDataType>(IConvention<TDataSource, TDataType> convention, TDataSource data, IConventionReportRenderer reporter) 
            where TDataSource : IConventionData, ICreateReportLineFor<TDataType>
        {
            var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);

            Reports.Add(conventionResult);

            new ConventionReportTraceRenderer().Render(conventionResult);
            reporter.Render(conventionResult);
        }

        public static void IsWithApprovedExeptions<TDataSource, TDataType>(IConvention<TDataSource, TDataType> convention, TDataSource data)
            where TDataSource : IConventionData, ICreateReportLineFor<TDataType>
        {
            var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);

            try
            {
                var conventionReportTextRenderer = new ConventionReportTextRenderer();
                conventionReportTextRenderer.RenderItems(conventionResult);
                conventionResult.WithApprovedException(conventionReportTextRenderer.Output);

                conventionReportTextRenderer.Render(conventionResult);
                Approvals.Verify(conventionReportTextRenderer.Output);

                new ConventionReportTraceRenderer().Render(conventionResult);
            }
            catch (ApprovalException ex)
            {
                throw new ConventionFailedException("Approved exceptions for convention differs\r\n\r\n"+ex.Message, ex);
            }
        }

        public static void Is<TDataSource, TDataType>(ISymmetricConvention<TDataSource, TDataType> convention, TDataSource data)
            where TDataSource : IConventionData, ICreateReportLineFor<TDataType>
        {
            Is(convention, data, new ConventionResultExceptionReporter());
        }

        public static void Is<TDataSource, TDataType>(ISymmetricConvention<TDataSource, TDataType> convention, TDataSource data, IConventionReportRenderer reporter)
            where TDataSource : IConventionData, ICreateReportLineFor<TDataType>
        {
            var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);
            var inverseConventionResult = GetConventionReport(convention.InverseTitle, convention.GetFailingInverseData(data).ToArray(), data);

            Reports.Add(conventionResult);
            Reports.Add(inverseConventionResult);

            new ConventionReportTraceRenderer().Render(conventionResult, inverseConventionResult);
            reporter.Render(conventionResult, inverseConventionResult);
        }

        public static void IsWithApprovedExeptions<TDataSource, TDataType>(ISymmetricConvention<TDataSource, TDataType> convention, TDataSource data)
            where TDataSource : IConventionData, ICreateReportLineFor<TDataType>
        {
            var conventionResult = GetConventionReport(convention.ConventionTitle, convention.GetFailingData(data).ToArray(), data);
            var inverseConventionResult = GetConventionReport(convention.InverseTitle, convention.GetFailingInverseData(data).ToArray(), data);

            try
            {
                var conventionReportTextRenderer = new ConventionReportTextRenderer();
                // Add approved exceptions to report
                conventionReportTextRenderer.RenderItems(conventionResult);
                conventionResult.WithApprovedException(conventionReportTextRenderer.Output);

                // Add approved exceptions to inverse report
                conventionReportTextRenderer.RenderItems(inverseConventionResult);
                inverseConventionResult.WithApprovedException(conventionReportTextRenderer.Output);

                //Render both, with approved exceptions included
                conventionReportTextRenderer.Render(conventionResult, inverseConventionResult);
                Approvals.Verify(conventionReportTextRenderer.Output);
                
                // Trace on success
                new ConventionReportTraceRenderer().Render(conventionResult, inverseConventionResult);
            }
            catch (ApprovalException ex)
            {
                throw new ConventionFailedException("Approved exceptions for convention differs\r\n\r\n" + ex.Message, ex);
            }
        }

        static ConventionReport GetConventionReport<TDataSource, TDataType>(string conventionTitle, TDataType[] failingData, TDataSource data)
            where TDataSource : IConventionData, ICreateReportLineFor<TDataType>
        {
            data.EnsureHasNonEmptySource();
            var passed = failingData.None();

            var conventionResult = new ConventionReport(
                passed ? Result.Passed : Result.Failed,
                conventionTitle,
                data.Description,
                failingData.Select(data.CreateReportLine));
            return conventionResult;
        }
    }
}