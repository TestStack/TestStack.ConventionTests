namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public interface ITestResultProcessor
    {
        string Process(IConventionFormatContext context, ConventionResult result);
        string RecommendedFileExtension { get; }
    }
}