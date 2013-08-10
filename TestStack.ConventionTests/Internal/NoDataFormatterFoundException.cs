namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NoDataFormatterFoundException : Exception
    {
        public NoDataFormatterFoundException()
        {
        }

        public NoDataFormatterFoundException(string message) : base(message)
        {
        }

        public NoDataFormatterFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NoDataFormatterFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}