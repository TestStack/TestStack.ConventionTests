namespace TestStack.ConventionTests.Internal
{
    using System;
    #if Serializable
    using System.Runtime.Serialization;
    #endif

    #if Serializable
    [System.Serializable]
    #endif
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

        #if Serializable
        protected NoDataFormatterFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
        #endif
    }
}