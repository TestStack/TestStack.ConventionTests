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

        protected override void Process(IConventionFormatContext context, IEnumerable<IGrouping<string, ConventionResult>> resultsGroupedByDataType)
        {
            var sb = new StringBuilder();
            var html = new HtmlTextWriter(new StringWriter(sb));
            html.WriteLine("<!DOCTYPE html>");
            html.RenderBeginTag(HtmlTextWriterTag.Html);  // <html>
            html.RenderBeginTag(HtmlTextWriterTag.Head);  // <head>


            html.AddAttribute("href", "http://netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/css/bootstrap-combined.min.css");
            html.AddAttribute("rel", "stylesheet");
            html.RenderBeginTag(HtmlTextWriterTag.Link);
            html.RenderEndTag();
            html.AddAttribute("href", "http://netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.css");
            html.AddAttribute("rel", "stylesheet");

            html.Write(@"
                       <style type=""text/css"">
                           h1 {
                               color: #3c6eb4;
                               border-bottom: 1px solid #3c6eb4;
                           }
                       </style>");

            html.RenderBeginTag(HtmlTextWriterTag.Link);
            html.RenderEndTag();

            html.RenderEndTag();                          // </head>
            html.RenderBeginTag(HtmlTextWriterTag.Body);  // <body>

            html.RenderBeginTag(HtmlTextWriterTag.H1);
            html.Write("Project Conventions");
            html.RenderEndTag();

            foreach (var conventionReport in resultsGroupedByDataType)
            {
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.AddAttribute("style", "margin-left:20px;border-bottom: 1px solid");
                html.RenderBeginTag(HtmlTextWriterTag.H2);
                html.Write("Conventions for '<strong>{0}</strong>'", conventionReport.Key);
                html.RenderEndTag();
                foreach (var conventionResult in conventionReport)
                {
                    var targetId = 
                        conventionResult.ConventionTitle.Replace("'", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty) +
                        conventionResult.DataDescription.Replace("'", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty);
                    html.AddAttribute("style", "margin-left:20px;");
                    html.RenderBeginTag(HtmlTextWriterTag.H4);

                    html.AddAttribute("class", "menu-toggle");
                    html.AddAttribute("data-toggle", "collapse");
                    html.AddAttribute("data-target", "." + targetId);
                    html.RenderBeginTag(HtmlTextWriterTag.A);
                    html.AddAttribute("class", "icon-angle-down");
                    html.RenderBeginTag(HtmlTextWriterTag.I);
                    html.RenderEndTag();
                    html.RenderEndTag();
                    html.Write(conventionResult.ConventionTitle);
                    html.RenderEndTag();
                    html.AddAttribute("class", targetId + " collapse");
                    html.AddAttribute("style", "margin-left:20px;");
                    html.RenderBeginTag(HtmlTextWriterTag.Div);
                    html.RenderBeginTag(HtmlTextWriterTag.Ul);
                    foreach (var o in conventionResult.Data)
                    {
                        html.RenderBeginTag(HtmlTextWriterTag.Li);
                        html.Write(context.FormatDataAsHtml(o));
                        html.RenderEndTag();
                    }
                    html.RenderEndTag();
                    html.RenderEndTag();
                }
                html.RenderEndTag();
            }

            html.AddAttribute("src", "http://code.jquery.com/jquery.js");
            html.RenderBeginTag(HtmlTextWriterTag.Script);
            html.RenderEndTag();
            html.AddAttribute("src", "http://netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js");
            html.RenderBeginTag(HtmlTextWriterTag.Script);
            html.RenderEndTag();
            html.RenderEndTag();                          // </body>
            html.RenderEndTag();                          // </html>
            html.Flush();

            File.WriteAllText(file, sb.ToString());
        }
    }
}