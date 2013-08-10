namespace TestStack.ConventionTests
{
    using System;
    using System.Diagnostics;
    using ApprovalTests;
    using ApprovalTests.Core.Exceptions;
    using TestStack.ConventionTests.Internal;

    public static class Convention
    {
        public static void Is<TData>(IConvention<TData> convention, TData data) where TData : IConventionData
        {
            var result = Execute(convention, data);

            if (result.Failed)
                throw new ConventionFailedException(result.Message);
        }

        static ConventionResult Execute<TData>(IConvention<TData> convention, TData data) where TData : IConventionData
        {
            data.EnsureHasNonEmptySource();
            var result = convention.Execute(data);
            return result;
        }

        public static void IsWithApprovedExeptions<TData>(IConvention<TData> convention, TData data)
            where TData : IConventionData
        {
            var result = Execute(convention, data);

            // should we encapsulate Approvals behind Settings?
            try
            {
                Approvals.Verify(result.Message);

                Trace.WriteLine(string.Format("{0} has approved exceptions:{2}{2}{1}",
                    convention.GetType().Name,
                    result.Message,
                    Environment.NewLine));
            }
            catch (ApprovalException ex)
            {
                throw new ConventionFailedException(
                    "Approved exceptions for convention differs" +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message, ex);
            }
        }
    }
}