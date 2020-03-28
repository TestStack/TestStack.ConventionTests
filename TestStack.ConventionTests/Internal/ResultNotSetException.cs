namespace TestStack.ConventionTests.Internal
{
    using System;
    #if Serializable
    using System.Runtime.Serialization;
    #endif

    #if Serializable
    [System.Serializable]
    #endif
    public class ResultNotSetException : Exception
    {
        public ResultNotSetException() { }
        public ResultNotSetException(string message) : base(message) { }
        public ResultNotSetException(string message, Exception inner) : base(message, inner) { }
        #if Serializable
        protected ResultNotSetException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
        #endif
    }
}