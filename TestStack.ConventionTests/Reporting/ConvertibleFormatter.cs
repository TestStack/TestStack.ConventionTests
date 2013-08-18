namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using TestStack.ConventionTests.Internal;

    public class ConvertibleFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is IConvertible;
        }

        public ConventionReportFailure Format(object failingData)
        {
            var convertible = failingData as IConvertible;
            Debug.Assert(convertible != null, "convertible != null");
            return new ConventionReportFailure(convertible.ToString(CultureInfo.InvariantCulture));
        }
    }
}