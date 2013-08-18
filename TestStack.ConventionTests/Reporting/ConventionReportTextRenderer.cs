namespace TestStack.ConventionTests.Reporting
{
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTextRenderer : IResultsProcessor
    {
        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            foreach (var result in results)
            {
                if (result.FormattedResult != null)
                {
                    continue;
                }
                var description = new StringBuilder();
                var title = string.Format("'{0}' for '{1}'", result.ConventionTitle,
                    result.DataDescription);
                description.AppendLine(title);
                description.AppendLine(string.Empty.PadRight(title.Length, '-'));
                description.AppendLine();

                RenderItems(result, description, context);
                result.WithFormattedResult(description.ToString());
            }
        }

        static void RenderItems(ConventionResult resultInfo, StringBuilder stringBuilder, IConventionFormatContext context)
        {
            foreach (var conventionFailure in resultInfo.Data)
            {
                stringBuilder.Append("\t");
                stringBuilder.AppendLine(context.FormatData(conventionFailure).ToString());
            }
        }
    }
}