namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ResultNotSetException : Exception
    {
        public ResultNotSetException() { }
        public ResultNotSetException(string message) : base(message) { }
        public ResultNotSetException(string message, Exception inner) : base(message, inner) { }

        protected ResultNotSetException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}