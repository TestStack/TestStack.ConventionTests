namespace TestStack.ConventionTests.Reporting
{
    using System.Reflection;

    public class MethodInfoDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is MethodInfo;
        }

        public string FormatString(object failingData)
        {
            var methodInfo = (MethodInfo)failingData;

            return methodInfo.DeclaringType + "." + methodInfo.Name;
        }
    }
}