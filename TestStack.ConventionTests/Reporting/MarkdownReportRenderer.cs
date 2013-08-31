namespace TestStack.ConventionTests.Reporting
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class MarkdownReportRenderer : GroupedByDataTypeRendererBase
    {
        readonly string file;

        public MarkdownReportRenderer(string assemblyDirectory)
        {
            file = Path.Combine(assemblyDirectory, "Conventions.md");
        }

        protected override void Process(IConventionFormatContext context, IEnumerable<IGrouping<string, ConventionResult>> resultsGroupedByDataType)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Project Conventions");
            foreach (var conventionReport in resultsGroupedByDataType)
            {
                sb.Append("## ");
                sb.AppendLine(conventionReport.Key);

                foreach (var conventionResult in conventionReport)
                {
                    sb.Append(" - ");
                    sb.AppendLine(conventionResult.ConventionTitle);
                }
            }

            File.WriteAllText(file, sb.ToString());
        }
    }
}