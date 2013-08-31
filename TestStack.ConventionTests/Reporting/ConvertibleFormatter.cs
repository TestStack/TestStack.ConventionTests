namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public class ConvertibleFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is IConvertible;
        }

        public string FormatString(object failingData)
        {
            var convertible = failingData as IConvertible;
            Debug.Assert(convertible != null, "convertible != null");
            return convertible.ToString(CultureInfo.InvariantCulture);
        }
    }
}