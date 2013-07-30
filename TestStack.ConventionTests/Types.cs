namespace TestStack.ConventionTests
{
    using System;

    /// <summary>
    ///     This is where we set what our convention is all about.
    /// </summary>
    // NOTE: notice no base type, no interface. Just a POCO DTO
    public class Types
    {
        //NOTE: that's a terrible name
        public Type[] ApplicableTypes { get; set; }

        public bool HasApprovedExceptions { get; set; }
    }
}