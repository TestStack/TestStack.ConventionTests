namespace TestStack.ConventionTests
{
    using System;
    using TestStack.ConventionTests.Reporting;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ConventionReporterAttribute : Attribute
    {
        public ConventionReporterAttribute(Type reporterType)
        {
            ReporterType = reporterType;
            if (!typeof(IResultsProcessor).IsAssignableFrom(reporterType))
                throw new ArgumentException("Reporters must inherit from IResultsProcessor", "reporterType");
        }

        public Type ReporterType { get; private set; }
    }
}