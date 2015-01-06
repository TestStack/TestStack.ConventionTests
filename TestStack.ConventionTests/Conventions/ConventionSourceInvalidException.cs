namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ConventionSourceInvalidException : Exception
    {
        public ConventionSourceInvalidException() { }
        public ConventionSourceInvalidException(string message) : base(message) { }
        public ConventionSourceInvalidException(string message, Exception inner) : base(message, inner) { }
        protected ConventionSourceInvalidException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }
}