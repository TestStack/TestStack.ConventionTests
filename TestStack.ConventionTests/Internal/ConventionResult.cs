namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class ConventionResult : IConventionResult
    {

        public string Message { get; private set; }

        public bool Failed
        {
            get { return !string.IsNullOrEmpty(Message); }
        }


        public IEnumerable<object> Items { get; set; }

        void IConventionResult.Is<TResult>(IEnumerable<TResult> items)
        {
            Items = items.Select(item => (object) item).ToList();
        }

        public void Is<TResult>(
            IEnumerable<TResult> items,
            Action<TResult, StringBuilder> itemDescriptor)
        {
            var array = items.ToArray();
            if (array.None())
            {
                return;
            }

            // NOTE: we might possibly want to abstract the StringBuilder to have more high level construct that would allow us to plug rich reports here...
            var message = new StringBuilder();
            message.AppendLine();
            message.AppendLine();
            Array.ForEach(array, r => itemDescriptor(r, message));
            Message = message.ToString();
        }

        public void Is<TResult>(
            IEnumerable<TResult> items,
            Func<TResult, string> itemDescriptor)
        {
            Is(items, (item, message) => message.AppendLine(itemDescriptor(item)));
        }

        public void IsSymmetric<TResult>(
            string firstHeader, TResult[] firstResults,
            string secondHeader, TResult[] secondResults,
            Action<TResult, StringBuilder> itemDescriptor)
        {
            var firstArray = firstResults.ToArray();
            var secondArray = secondResults.ToArray();
            if (firstArray.None() && secondArray.None())
            {
                return;
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
            Message = message.ToString();
        }

        public void IsSymmetric<TResult>(
            string firstHeader, TResult[] firstResults,
            string secondHeader, TResult[] secondResults,
            Func<TResult, string> itemDescriptor)
        {
            IsSymmetric(
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
        readonly PropertyInfo[] properties;
        readonly Type type;

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