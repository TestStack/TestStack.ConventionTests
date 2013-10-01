namespace TestStack.ConventionTests.Reporting
{
    using System;
    using TestStack.ConventionTests.ConventionData;

    public class TypeDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return data is Type;
        }

        public string FormatString(object data)
        {
            return ((Type)data).ToTypeNameString();
        }

        public string FormatHtml(object data)
        {
            var type = ((Type)data);
            var oldValue = string.Format("{0}.", type.Namespace);
            var newValue = string.Format("{0}.<strong>", type.Namespace);
            return string.Format("{0}</strong>", FormatString(data).Replace(oldValue, newValue));
        }
    }
}