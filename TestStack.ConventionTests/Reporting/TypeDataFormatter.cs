namespace TestStack.ConventionTests.Reporting
{
    using System;
    using TestStack.ConventionTests.Internal;

    public class TypeDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is Type;
        }

        public ConventionReportFailure Format(object failingData)
        {
            return new ConventionReportFailure(((Type)failingData).FullName);
        }
    }
}