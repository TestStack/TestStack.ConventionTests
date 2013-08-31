namespace TestStack.ConventionTests.Reporting
{
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class CsvReporter : ITestResultProcessor
    {
        public string Process(IConventionFormatContext context, ConventionResult result)
        {
            var formatter = new DefaultFormatter(result.DataType);
            var message = new StringBuilder();
            message.AppendLine(string.Join(",", formatter.DesribeType()));
            foreach (var item in result.Data)
            {
                message.AppendLine(string.Join(",", formatter.DesribeItem(item, context)));
            }
            return message.ToString();
        }

        public string RecommendedFileExtension { get { return "csv"; } }
    }
}