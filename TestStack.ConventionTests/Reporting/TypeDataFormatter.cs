namespace TestStack.ConventionTests.Reporting
{
    using System;

    public class TypeDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is Type;
        }

        public string FormatString(object failingData)
        {
            return ((Type)failingData).FullName;
        }
    }
}