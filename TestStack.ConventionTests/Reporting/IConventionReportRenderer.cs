namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public interface IConventionReportRenderer
    {
        void Render(params ConventionResult[] conventionResult);
    }
}