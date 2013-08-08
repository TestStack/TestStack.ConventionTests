namespace TestStack.ConventionTests
{
    public interface IConventionReportRenderer
    {
        void Render(params ConventionReport[] conventionResult);
    }
}