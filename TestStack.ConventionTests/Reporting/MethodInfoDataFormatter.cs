namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Reflection;
    using System.Text;

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
                    sb.Append(TypeName(g));
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

        static string TypeName(Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                return nullableType.Name + "?";

            if (!type.IsGenericType)
                switch (type.Name)
                {
                    case "String":
                        return "string";
                    case "Int32":
                        return "int";
                    case "Decimal":
                        return "decimal";
                    case "Object":
                        return "object";
                    case "Void":
                        return "void";
                    default:
                        {
                            return string.IsNullOrWhiteSpace(type.FullName) ? type.Name : type.FullName;
                        }
                }

            var sb = new StringBuilder(type.Name.Substring(0,
            type.Name.IndexOf('`'))
            );
            sb.Append('<');
            var first = true;
            foreach (var t in type.GetGenericArguments())
            {
                if (!first)
                    sb.Append(',');
                sb.Append(TypeName(t));
                first = false;
            }
            sb.Append('>');
            return sb.ToString();
        }
    }
}