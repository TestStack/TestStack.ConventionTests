namespace TestStack.ConventionTests.Reporting
{
    using System.Reflection;
    using TestStack.ConventionTests.Internal;

    public class MethodInfoDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is MethodInfo;
        }

        public ConventionReportFailure Format(object failingData)
        {
            var methodInfo = (MethodInfo)failingData;

            return new ConventionReportFailure(methodInfo.DeclaringType + "." + methodInfo.Name);
        }
    }
}