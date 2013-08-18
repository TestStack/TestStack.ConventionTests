namespace TestStack.ConventionTests.Reporting
{
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTextRenderer : IResultsProcessor
    {
        public void Process(IConventionFormatContext context, params ConventionResult[] results)
        {
            var stringBuilder = new StringBuilder();

            foreach (var conventionReport in results)
            {
                var title = string.Format("'{0}' for '{1}'", conventionReport.ConventionTitle,
                    conventionReport.DataDescription);
                stringBuilder.AppendLine(title);
                stringBuilder.AppendLine(string.Empty.PadRight(title.Length, '-'));
                stringBuilder.AppendLine();

                RenderItems(conventionReport, stringBuilder, context);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
            }

            Output = stringBuilder.ToString().TrimEnd();
        }

        public string Output { get; private set; }

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