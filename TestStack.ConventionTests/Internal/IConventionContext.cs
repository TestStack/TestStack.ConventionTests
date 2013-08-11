namespace TestStack.ConventionTests.Internal
{
    using System.Collections.Generic;
    using TestStack.ConventionTests.Reporting;

    public interface IConventionContext
    {
        IEnumerable<IReportDataFormatter> Formatters { get; }
        IEnumerable<IConventionReportRenderer> Renderers { get; }
        IConventionData Data { get; }
    }
}