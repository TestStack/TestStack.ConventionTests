namespace TestStack.ConventionTests.Reporting
{
    using System.Collections;
    using System.Text;

    public class CsvReporter
    {
        public string Build(IEnumerable results, string header, DefaultFormatter formatter)
        {
            var message = new StringBuilder();
            message.AppendLine(string.Join(",", formatter.DesribeType()));
            foreach (var result in results)
            {
                message.AppendLine(string.Join(",", formatter.DesribeItem(result)));
            }
            return message.ToString();
        }
    }
}