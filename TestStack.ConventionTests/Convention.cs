namespace TestStack.ConventionTests
{
    using System.Diagnostics;
    using ApprovalTests;
    using ApprovalTests.Core.Exceptions;

    public static class Convention
    {
        public static void Is<TData>(IConvention<TData> convention, TData data) where TData : IConventionData
        {
            data.EnsureHasNonEmptySource();
            var result = convention.Execute(data);

            if (result.Failed)
                throw new ConventionFailedException(result.Message);
        }

        public static void IsWithApprovedExeptions<TData>(IConvention<TData> convention, TData data) where TData : IConventionData
        {
            data.EnsureHasNonEmptySource();
            var result = convention.Execute(data);

            // should we encapsulate Approvals behind Settings?
            try
            {
                Approvals.Verify(result.Message);

                Trace.WriteLine(string.Format("{0} has approved exceptions:\r\n\r\n{1}", convention.GetType().Name, result.Message));
            }
            catch (ApprovalException ex)
            {
                throw new ConventionFailedException("Approved exceptions for convention differs\r\n\r\n"+ex.Message, ex);
            }
        }
    }
}