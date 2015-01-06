namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class ThrowOnFailureResultsProcessor : IResultsProcessor
    {
        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            var invalidResults = results.Where(r => r.HasData).Select(r => context.TestResultProcessor.Process(context, r)).ToArray();
            if (invalidResults.None())
            {
                return;
            }
            throw new ConventionFailedException(string.Join(Environment.NewLine, invalidResults));
        }
    }
}