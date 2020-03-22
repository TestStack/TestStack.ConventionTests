namespace TestStack.ConventionTests
{
    using System;
    using System.Runtime.Serialization;

    public class ConventionFailedException : Exception
    {
        public ConventionFailedException()
        {
        }

        public ConventionFailedException(string message) : base(message)
        {
        }

        public ConventionFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConventionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}