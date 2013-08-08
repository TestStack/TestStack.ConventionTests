namespace TestStack.ConventionTests
{
    public interface ICreateReportLineFor<in T>
    {
        ConventionFailure CreateReportLine(T failingData);
    }
}