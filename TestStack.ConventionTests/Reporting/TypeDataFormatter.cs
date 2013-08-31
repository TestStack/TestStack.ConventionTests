namespace TestStack.ConventionTests.Reporting
{
    using System;

    public class TypeDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return data is Type;
        }

        public string FormatString(object data)
        {
            return ((Type)data).FullName;
        }

        public string FormatHtml(object data)
        {
            var type = ((Type)data);
            var ns = type.Namespace;
            if (ns == null)
                return type.Name;
            return string.Format("{0}.<strong>{1}</strong>", ns, type.Name);
        }
    }
}