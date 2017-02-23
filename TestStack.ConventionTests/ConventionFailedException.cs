namespace TestStack.ConventionTests
{
    using System;
    #if Serializable
    using System.Runtime.Serialization;
    #endif

    #if Serializable
    [System.Serializable]
    #endif
    public class ConventionFailedException : Exception
    {
        public ConventionFailedException() { }
        public ConventionFailedException(string message) : base(message) { }
        public ConventionFailedException(string message, Exception inner) : base(message, inner) { }

        #if Serializable
        protected ConventionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        #endif
    }
}