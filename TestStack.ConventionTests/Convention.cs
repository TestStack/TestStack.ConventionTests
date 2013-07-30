namespace TestStack.ConventionTests
{
    using System;
    using ApprovalTests;

    public static class Convention
    {
        public static readonly ConventionSettings Settings = new ConventionSettings();

        public static void Is<TData>(IConvention<TData> convention, TData data) where TData : IConventionData
        {
            if (data.HasValidSource == false)
            {
                // TODO: this would have to have a more reasonable and helpful message...
                Settings.AssertInclunclusive("No valid source in " + data);
                return;
            }
            var result = convention.Execute(data);
            if (result.IsConclusive == false)
            {
                Settings.AssertInclunclusive(result.Message);
                return;
            }
            if (data.HasApprovedExceptions)
            {
                // should we encapsulate Approvals behind Settings?
                Approvals.Verify(result.Message);
                return;
            }
            Settings.AssertZero(result.InvalidResultsCount, result.Message);
        }

        public class ConventionSettings
        {
            public Action<String> AssertInclunclusive;

            public Action<int, string> AssertZero;

            public ConventionSettings()
            {
                // TODO: initialize the type;
            }
        }
    }
}