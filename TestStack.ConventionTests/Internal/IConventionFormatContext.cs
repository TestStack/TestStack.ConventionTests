namespace TestStack.ConventionTests.Internal
{
    using TestStack.ConventionTests.Reporting;

    public interface IConventionFormatContext
    {
        string FormatDataAsString(object failingData);
        ITestResultProcessor TestResultProcessor { get; }
    }
}