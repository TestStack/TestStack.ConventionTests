namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConventionResult
    {
        ConventionResult(bool failed)
        {
            Failed = failed;
        }

        public string Message { get; private set; }

        public bool Failed { get; private set; }

        public static ConventionResult For<TResult>(
            string header, IEnumerable<TResult> items, 
            Action<TResult, StringBuilder> itemDescriptor)
        {
            var array = items.ToArray();
            if (array.None())
            {
                return new ConventionResult(failed: false);
            }

            var result = new ConventionResult(true);
            var message = new StringBuilder(header);
            message.AppendLine();
            message.AppendLine(string.Empty.PadRight(header.Length, '-'));
            message.AppendLine();
            Array.ForEach(array, r => itemDescriptor(r, message));
            result.Message = message.ToString();
            return result;
        }

        public static ConventionResult For<TResult>(
            IEnumerable<TResult> items, string header,
            Func<TResult, string> itemDescriptor)
        {
            return For(header, items, (item, message) => message.AppendLine(itemDescriptor(item)));
        }

        public static ConventionResult ForSymmetric<TResult>(
            string firstHeader, TResult[] firstResults,
            string secondHeader, TResult[] secondResults,
            Action<TResult, StringBuilder> itemDescriptor)
        {
            var firstArray = firstResults.ToArray();
            var secondArray = secondResults.ToArray();
            if (firstArray.None() && secondArray.None())
            {
                return new ConventionResult(failed: true);
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
}