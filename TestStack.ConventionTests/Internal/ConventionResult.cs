namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConventionResult
    {
        ConventionResult() { }

        public string Message { get; private set; }
        public bool Failed { get { return !string.IsNullOrEmpty(Message); }}

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
            message.AppendLine();
            Array.ForEach(array, r => itemDescriptor(r, message));
            result.Message = message.ToString();
            return result;
        }
    }
}