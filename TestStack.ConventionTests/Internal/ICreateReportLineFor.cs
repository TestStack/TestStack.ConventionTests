namespace TestStack.ConventionTests.Internal
{
    public interface ICreateReportLineFor<in T>
    {
        ConventionFailure CreateReportLine(T failingData);
    }
}