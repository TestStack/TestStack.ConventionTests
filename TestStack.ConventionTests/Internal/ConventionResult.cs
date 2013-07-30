namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConventionResult
    {
        public string Message { get; set; }
        public bool IsConclusive { get; set; }
        // TODO: perhaps name it better so that it doesn't get confused with System.Exception and related concepts
        public bool HasExceptions { get; set; }
        public int InvalidResultsCount { get; set; }

        public static ConventionResult For<TResult>(IEnumerable<TResult> items,
            string header,
            Action<TResult, StringBuilder> itemDescriptor)
        {
            var array = items.ToArray();
            var result = new ConventionResult
            {
                InvalidResultsCount = array.Length,
                IsConclusive = true
            };
            if (array.None())
            {
                return result;
            }
            // NOTE: we might possibly want to abstract the StringBuilder to have more high level construct that would allow us to plug rich reports here...
            var message = new StringBuilder(header);
            Array.ForEach(array, r =>
            {
                message.AppendLine();
                itemDescriptor(r, message);
            });
            result.Message = message.ToString();
            return result;
        }

        public static ConventionResult Inconclusive(string message)
        {
            return new ConventionResult
            {
                Message = message,
                IsConclusive = false
            };
        }
    }
}