namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class ConventionResult
    {
        ConventionResult()
        {
        }

        public string Message { get; private set; }

        public bool Failed
        {
            get { return !string.IsNullOrEmpty(Message); }
        }

        public static ConventionResult For<TResult>(
            IEnumerable<TResult> items,
            string header)
        {
            var array = items.ToArray();
            var result = new ConventionResult();
            if (array.None())
            {
                return result;
            }
            // ROUGHSKETCH: We split the job between formatter and reporter. Formatter provides the data, reporter provides the structure
            var formatter = new DefaultFormatter(typeof (TResult));
            var reporter = new CsvReporter();
            return new ConventionResult
            {
                Message = reporter.Build(array, header, formatter)
            };
        }

        public static ConventionResult For<TResult>(
            IEnumerable<TResult> items,
            string header,
            Action<TResult, StringBuilder> itemDescriptor)
        {
            var array = items.ToArray();
            var result = new ConventionResult();
            if (array.None())
            {
                return result;
            }

            // NOTE: we might possibly want to abstract the StringBuilder to have more high level construct that would allow us to plug rich reports here...
            var message = new StringBuilder(header);
            message.AppendLine();
            message.AppendLine(string.Empty.PadRight(header.Length, '-'));
            message.AppendLine();
            Array.ForEach(array, r => itemDescriptor(r, message));
            result.Message = message.ToString();
            return result;
        }

        public static ConventionResult For<TResult>(
            IEnumerable<TResult> items,
            string header,
            Func<TResult, string> itemDescriptor)
        {
            return For(items, header, (item, message) => message.AppendLine(itemDescriptor(item)));
        }

        public static ConventionResult ForSymmetric<TResult>(
            string firstHeader, TResult[] firstResults,
            string secondHeader, TResult[] secondResults,
            Action<TResult, StringBuilder> itemDescriptor)
        {
            var firstArray = firstResults.ToArray();
            var secondArray = secondResults.ToArray();
            var result = new ConventionResult();
            if (firstArray.None() && secondArray.None())
            {
                return result;
            }

            var message = new StringBuilder();
            if (firstArray.Any())
            {
                message.AppendLine(firstHeader);
                message.AppendLine(string.Empty.PadRight(firstHeader.Length, '-'));
                message.AppendLine();
                Array.ForEach(firstArray, r => itemDescriptor(r, message));
            }
            if (secondArray.Any())
            {
                if (firstArray.Any())
                {
                    message.AppendLine();
                    message.AppendLine();
                }
                message.AppendLine(secondHeader);
                message.AppendLine(string.Empty.PadRight(secondHeader.Length, '-'));
                message.AppendLine();
                Array.ForEach(secondArray, r => itemDescriptor(r, message));
            }
            result.Message = message.ToString();
            return result;
        }

        public static ConventionResult ForSymmetric<TResult>(
            string firstHeader, TResult[] firstResults,
            string secondHeader, TResult[] secondResults,
            Func<TResult, string> itemDescriptor)
        {
            return ForSymmetric(
                firstHeader, firstResults, secondHeader,
                secondResults,
                (item, message) => message.AppendLine(itemDescriptor(item)));
        }
    }

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

    public class DefaultFormatter
    {
        readonly Type type;
        readonly PropertyInfo[] properties;

        public DefaultFormatter(Type type)
        {
            this.type = type;
            properties = type.GetProperties();
        }

        // TODO: this is a very crappy name for a method
        public string[] DesribeType()
        {
            return properties.Select(Describe).ToArray();
        }

        string Describe(PropertyInfo property)
        {
            return property.Name.Replace('_', ' ');
        }

        public string[] DesribeItem(object result)
        {
            return properties.Select(p => p.GetValue(result, null).ToString()).ToArray();
        }
    }
}