namespace TestStack.ConventionTests.Reporting
{
    using System.ComponentModel;
    using System.Reflection;
    using ApprovalTests.Namers;
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