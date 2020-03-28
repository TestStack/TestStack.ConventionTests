namespace TestStack.ConventionTests
{
    using System;
    #if NewReflection
    using System.Reflection;
    #endif
    using TestStack.ConventionTests.Reporting;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class ConventionReporterAttribute : Attribute
    {
        public ConventionReporterAttribute(Type reporterType)
        {
            ReporterType = reporterType;
            if (!typeof(IResultsProcessor).IsAssignableFrom(reporterType))
                throw new ArgumentException("Reporters must inherit from IResultsProcessor", nameof(reporterType));
        }

        public Type ReporterType { get; }
    }
}