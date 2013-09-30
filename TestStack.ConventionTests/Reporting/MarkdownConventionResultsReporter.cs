namespace TestStack.ConventionTests.Reporting
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class MarkdownConventionResultsReporter : GroupedByDataTypeConventionResultsReporterBase
    {
        public MarkdownConventionResultsReporter() : base("Conventions.md")
        {
        }

        protected override string Process(IConventionFormatContext context, IEnumerable<IGrouping<string, ConventionResult>> resultsGroupedByDataType)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Project Conventions");
            foreach (var conventionReport in resultsGroupedByDataType)
            {
                sb.Append("## ");
                sb.AppendLine(conventionReport.Key);

                foreach (var conventionResult in conventionReport)
                {
                    sb.Append(" - **");
                    sb.Append(conventionResult.ConventionTitle);
                    sb.AppendLine("**  ");
                    sb.AppendLine(conventionResult.ConventionReason);
                }
            }

            return sb.ToString();
        }
    }
}