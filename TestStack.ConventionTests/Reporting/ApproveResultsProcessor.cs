namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ApprovalTests;
    using ApprovalTests.Core.Exceptions;
    using TestStack.ConventionTests.Internal;

    public class ApproveResultsProcessor : IResultsProcessor
    {
        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            var failedApprovals = new List<ApprovalException>();
            for (var count = 0; count < results.Length; count++)
            {
                var result = results[count];
                try
                {
                    //TODO Law of Demeter
                    var formattedResult = context.TestResultProcessor.Process(context, result);
                    var recommendedFileExtension = context.TestResultProcessor.RecommendedFileExtension;
                    Approvals.Verify(new ConventionTestsApprovalTextWriter(formattedResult, count, recommendedFileExtension));
                }
                catch (ApprovalException ex)
                {
                    failedApprovals.Add(ex);
                }
            }
            if (failedApprovals.Count == 0)
            {
                return;
            }
            if (failedApprovals.Count == 1)
            {
                var ex = failedApprovals[0];
                throw new ConventionFailedException("Approved exceptions for convention differs" +
                                                    Environment.NewLine +
                                                    Environment.NewLine +
                                                    ex.Message, ex);
            }
            throw new ConventionFailedException("Approved exceptions for convention differs" +
                                                Environment.NewLine +
                                                Environment.NewLine +
                                                string.Join(Environment.NewLine,
                                                    failedApprovals.Select(x => x.Message)),
                new AggregateException(failedApprovals.ToArray()));
        }
    }
}