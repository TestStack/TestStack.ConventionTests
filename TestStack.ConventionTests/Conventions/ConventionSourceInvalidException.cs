namespace TestStack.ConventionTests.Conventions
{
    using System;
    #if Serializable
    using System.Runtime.Serialization;
    #endif

    #if Serializable
    [Serializable]
    #endif
    public class ConventionSourceInvalidException : Exception
    {
        public ConventionSourceInvalidException() { }
        public ConventionSourceInvalidException(string message) : base(message) { }
        public ConventionSourceInvalidException(string message, Exception inner) : base(message, inner) { }

        #if Serializable
        protected ConventionSourceInvalidException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
        #endif
    }
}