namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class CaptureFailuresProcessor : IResultsProcessor
    {
        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            var failedApprovals = results.Select(result => context.TestResultProcessor.Process(context, result)).ToList();
            switch (failedApprovals.Count)
            {
                case 0:
                    Failures = string.Empty;
                    break;
                case 1:
                    Failures = failedApprovals[0];
                    break;
                default:
                    Failures = string.Join(Environment.NewLine, failedApprovals.ToArray());
                    break;
            }
        }

        /// <summary>
        /// The captured failures
        /// </summary>
        public string Failures { get; private set; }
    }
}