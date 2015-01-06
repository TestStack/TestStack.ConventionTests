namespace TestStack.ConventionTests.Reporting
{
    using System.Reflection;
    using System.Text;
    using TestStack.ConventionTests.ConventionData;

    public class MethodInfoDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object data)
        {
            return data is MethodInfo;
        }

        public string FormatString(object data)
        {
            var methodInfo = (MethodInfo)data;

            return methodInfo.DeclaringType + "." + methodInfo.Name;
        }

        public string FormatHtml(object data)
        {
            const string keywordFormat = "<span style=\"color: #0000FF\">{0}</span>";
            const string typeFormat = "<span style=\"color: #2B91AF\">{0}</span>";

            var methodInfo = (MethodInfo)data;
            var sb = new StringBuilder();
            var declaringType = methodInfo.DeclaringType;
            sb.AppendFormat("{0} {1}.{2} {{ ",
                string.Format(keywordFormat, "class"),
                declaringType.Namespace,
                string.Format(typeFormat, declaringType.Name));

            AppendAccess(methodInfo, sb, keywordFormat);

            if (methodInfo.IsVirtual)
            {
                sb.AppendFormat(keywordFormat, "virtual");
                sb.Append(" ");
            }

            AppendMethodName(methodInfo, sb);
            sb.Append(" (...)");
            sb.Append("}}");

            return sb.ToString();
        }

        void AppendMethodName(MethodInfo methodInfo, StringBuilder sb)
        {
            sb.Append(methodInfo.Name);
            bool firstParam = true;
            if (methodInfo.IsGenericMethod)
            {
                sb.Append("<");
                foreach (var g in methodInfo.GetGenericArguments())
                {
                    if (firstParam)
                        firstParam = false;
                    else
                        sb.Append(", ");
                    sb.Append(g.ToTypeNameString());
                }
                sb.Append(">");
            }
        }

        void AppendAccess(MethodInfo method, StringBuilder sb, string format = "{0}")
        {
            if (method.IsPublic)
                sb.AppendFormat(format, "public ");
            else if (method.IsPrivate)
                sb.AppendFormat("private ");
            else if (method.IsAssembly)
                sb.AppendFormat("internal ");
            if (method.IsFamily)
                sb.AppendFormat("protected ");
            if (method.IsStatic)
                sb.AppendFormat("static ");
        }
    }
}