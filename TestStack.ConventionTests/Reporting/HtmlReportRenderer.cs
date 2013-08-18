namespace TestStack.ConventionTests.Reporting
{
    using System.IO;
    using System.Text;
    using System.Web.UI;
    using TestStack.ConventionTests.Internal;

    public class HtmlReportRenderer : IResultsProcessor
    {
        readonly string file;

        public HtmlReportRenderer(string assemblyDirectory)
        {
            file = Path.Combine(assemblyDirectory, "Conventions.htm");
        }

        public void Process(params ConventionResult[] conventionResult)
        {
            var sb = new StringBuilder();
            var html = new HtmlTextWriter(new StringWriter(sb));
            html.WriteLine("<!DOCTYPE html>");
            html.RenderBeginTag(HtmlTextWriterTag.Html);  // <html>
            html.RenderBeginTag(HtmlTextWriterTag.Head);  // <head>
            html.RenderEndTag();                          // </head>
            html.WriteLine();
            html.RenderBeginTag(HtmlTextWriterTag.Body);  // <body>

            html.RenderBeginTag(HtmlTextWriterTag.H1);
            html.Write("Project Conventions");
            html.RenderEndTag();

            foreach (var conventionReport in conventionResult)
            {
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.RenderBeginTag(HtmlTextWriterTag.Strong);
                html.Write(conventionReport.Result+": ");
                html.RenderEndTag();
                var title = string.Format("{0} for {1}", conventionReport.ConventionTitle, conventionReport.DataDescription);
                html.Write(title);
                if (!string.IsNullOrEmpty(conventionReport.ApprovedException))
                {
                    html.RenderBeginTag(HtmlTextWriterTag.Div);
                    html.RenderBeginTag(HtmlTextWriterTag.Strong);
                    html.WriteLine("With approved exceptions:");
                    html.RenderEndTag();
                    html.RenderEndTag();
                }
                
                html.RenderBeginTag(HtmlTextWriterTag.Ul);

                if (!string.IsNullOrEmpty(conventionReport.ApprovedException))
                {
                    html.RenderBeginTag(HtmlTextWriterTag.Li);
                    html.WriteLine(conventionReport.ApprovedException);
                    html.RenderEndTag();
                }

                foreach (var conventionFailure in conventionReport.ConventionFailures)
                {
                    html.RenderBeginTag(HtmlTextWriterTag.Li);
                    html.Write(conventionFailure.ToString());
                    html.RenderEndTag();
                }

                html.RenderEndTag();
                html.RenderEndTag();
                html.RenderEndTag();
            }

            html.RenderEndTag();                          // </body>
            html.RenderEndTag();                          // </html>
            html.Flush();

            File.WriteAllText(file, sb.ToString());
        }
    }
}