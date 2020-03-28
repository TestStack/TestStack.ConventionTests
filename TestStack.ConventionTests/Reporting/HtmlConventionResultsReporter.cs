namespace TestStack.ConventionTests.Reporting
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class HtmlConventionResultsReporter : GroupedByDataTypeConventionResultsReporterBase
    {
        public HtmlConventionResultsReporter() : base("Conventions.htm")
        {
        }

        protected override string Process(IConventionFormatContext context, IEnumerable<IGrouping<string, ConventionResult>> resultsGroupedByDataType)
        {
            var sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>");
            sb.Append("<html>");  // <html>
            sb.Append("<head>");  // <head>
            
            sb.AppendLine(@"<link href=""http://netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/css/bootstrap-combined.min.css"" rel=""stylesheet"" />");

            sb.AppendLine(@"<link href=""http://netdna.bootstrapcdn.com/font-awesome/3.2.1/css/font-awesome.css"" rel=""stylesheet"" />");


            sb.Append(@"
            <style type=""text/css"">
                h1 {
                    color: #3c6eb4;
                    border-bottom: 1px solid #3c6eb4;
                }
            </style>");

            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h1>Project Conventions</h1>");

            // foreach (var conventionReport in resultsGroupedByDataType)
            // {
            //     html.RenderBeginTag(HtmlTextWriterTag.Div);
            //     html.AddAttribute("style", "margin-left:20px;border-bottom: 1px solid");

            //     html.RenderBeginTag(HtmlTextWriterTag.H2);
            //     html.Write("Conventions for '<strong>{0}</strong>'", conventionReport.Key);
            //     html.RenderEndTag();

            //     foreach (var conventionResult in conventionReport)
            //     {
            //         var targetId = 
            //             conventionResult.ConventionTitle.Replace("'", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty) +
            //             conventionResult.DataDescription.Replace("'", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty);
            //         html.AddAttribute("style", "margin-left:20px;");
            //         html.RenderBeginTag(HtmlTextWriterTag.H4);
            //         html.Write(conventionResult.ConventionTitle);
            //         html.RenderEndTag();
            //         html.AddAttribute("style", "margin-left:20px;");
            //         html.RenderBeginTag(HtmlTextWriterTag.Div);
            //         html.Write(conventionResult.ConventionReason);
            //         html.RenderEndTag();
            //         if (conventionResult.Data.Any())
            //         {
            //             html.AddAttribute("style", "margin-left:20px;");
            //             html.RenderBeginTag(HtmlTextWriterTag.Div);
            //                 html.AddAttribute("class", "menu-toggle");
            //                 html.AddAttribute("data-toggle", "collapse");
            //                 html.AddAttribute("data-target", "." + targetId);
            //                 html.RenderBeginTag(HtmlTextWriterTag.A);
            //                     html.AddAttribute("class", "icon-angle-down");
            //                     html.RenderBeginTag(HtmlTextWriterTag.I);
            //                     html.RenderEndTag();
            //                     html.Write("With the exception of the following {0}: ", conventionResult.DataType.GetSentenceCaseName());
            //                 html.RenderEndTag();
            //             html.AddAttribute("class", targetId + " collapse");
            //             html.AddAttribute("style", "margin-left:20px;");
            //             html.RenderBeginTag(HtmlTextWriterTag.Div);
            //                 html.RenderBeginTag(HtmlTextWriterTag.Ul);
            //                 foreach (var o in conventionResult.Data)
            //                 {
            //                     html.RenderBeginTag(HtmlTextWriterTag.Li);
            //                     html.Write(context.FormatDataAsHtml(o));
            //                     html.RenderEndTag();
            //                 }
            //                 html.RenderEndTag();
            //             html.RenderEndTag();
            //         }
            //     }
            //     html.RenderEndTag();
            // }

            // html.AddAttribute("src", "http://code.jquery.com/jquery.js");
            // html.RenderBeginTag(HtmlTextWriterTag.Script);
            // html.RenderEndTag();

            // html.AddAttribute("src", "http://netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js");
            // html.RenderBeginTag(HtmlTextWriterTag.Script);
            // html.RenderEndTag();

            // html.RenderEndTag();                          // </body>
            // html.RenderEndTag();                          // </html>
            // html.Flush();
            return sb.ToString();
        }
    }
}