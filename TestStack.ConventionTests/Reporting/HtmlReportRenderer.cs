namespace TestStack.ConventionTests.Reporting
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using TestStack.ConventionTests.Internal;

    public class HtmlReportRenderer : GroupedByDataTypeRendererBase
    {
        readonly string file;

        public HtmlReportRenderer(string assemblyDirectory)
        {
            file = Path.Combine(assemblyDirectory, "Conventions.htm");
        }

        protected override void Process(IEnumerable<IGrouping<string, ConventionResult>> resultsGroupedByDataType)
        {
            var sb = new StringBuilder();
            var html = new HtmlTextWriter(new StringWriter(sb));
            html.WriteLine("<!DOCTYPE html>");
            html.RenderBeginTag(HtmlTextWriterTag.Html);  // <html>
            html.RenderBeginTag(HtmlTextWriterTag.Head);  // <head>
            html.Write(@"
           <style type=""text/css"">
               h1 {
                   color: #3c6eb4;
               }

               h2 {
                   background: #f1f1f1;
               }
           </style>");
            html.RenderEndTag();                          // </head>
            html.WriteLine();
            html.RenderBeginTag(HtmlTextWriterTag.Body);  // <body>

            html.RenderBeginTag(HtmlTextWriterTag.H1);
            html.Write("Project Conventions");
            html.RenderEndTag();

            foreach (var conventionReport in resultsGroupedByDataType)
            {
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.Write("Conventions for '<strong>{0}</strong>'", conventionReport.Key);
                html.RenderEndTag();
                html.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var conventionResult in conventionReport)
                {
                    html.RenderBeginTag(HtmlTextWriterTag.Li);
                    html.Write(conventionResult.ConventionTitle);
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