namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConventionResult
    {
        public string Message { get; set; }

        public static ConventionResult For<TResult>(IEnumerable<TResult> items,
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
            Array.ForEach(array, r =>
            {
                message.AppendLine();
                itemDescriptor(r, message);
            });
            result.Message = message.ToString();
            return result;
        }
    }
}