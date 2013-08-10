namespace TestStack.ConventionTests.Reporting
{
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTextRenderer : IConventionReportRenderer
    {
        public void Render(params ResultInfo[] conventionResult)
        {
            var stringBuilder = new StringBuilder();

            foreach (var conventionReport in conventionResult)
            {
                var title = string.Format("{0}: '{1}' for '{2}'", conventionReport.Result, conventionReport.ConventionTitle,
                    conventionReport.DataDescription);
                stringBuilder.AppendLine(title);
                stringBuilder.AppendLine(string.Empty.PadRight(title.Length, '-'));
                stringBuilder.AppendLine();

                if (!string.IsNullOrEmpty(conventionReport.ApprovedException))
                {
                    stringBuilder.AppendLine("With approved exceptions:");
                    stringBuilder.AppendLine(conventionReport.ApprovedException);
                    stringBuilder.AppendLine();
                }

                RenderItems(conventionReport, stringBuilder);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
            }

            Output = stringBuilder.ToString().TrimEnd();
        }

        public string Output { get; private set; }

        public void RenderItems(ResultInfo conventionResult)
        {
            var stringBuilder = new StringBuilder();
            RenderItems(conventionResult, stringBuilder);
            Output = stringBuilder.ToString();
        }

        static void RenderItems(ResultInfo resultInfo, StringBuilder stringBuilder)
        {
            foreach (var conventionFailure in resultInfo.ConventionFailures)
            {
                stringBuilder.Append("\t");
                stringBuilder.AppendLine(conventionFailure.ToString());
            }
        }
    }
}