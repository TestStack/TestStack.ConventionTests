namespace TestStack.ConventionTests.Internal
{
    using TestStack.ConventionTests.Reporting;

    public interface IConventionFormatContext
    {
        string FormatDataAsString(object data);
        string FormatDataAsHtml(object data);
        ITestResultProcessor TestResultProcessor { get; }
    }
}