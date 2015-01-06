namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public class ConvertibleFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return data is IConvertible;
        }

        public string FormatString(object data)
        {
            var convertible = data as IConvertible;
            Debug.Assert(convertible != null, "convertible != null");
            return convertible.ToString(CultureInfo.InvariantCulture);
        }

        public string FormatHtml(object data)
        {
            return FormatString(data);
        }
    }
}