namespace TestStack.ConventionTests.Conventions
{
    using System;
    using TestStack.ConventionTests.Internal;

    /// <summary>
    ///     This is where we set what our convention is all about.
    /// </summary>
    public class Types : IConventionData
    {
        //NOTE: that's a terrible name
        public Type[] ApplicableTypes { get; set; }

        public bool HasApprovedExceptions { get; set; }

        public void ThrowIfHasInvalidSource()
        {
            if (ApplicableTypes.None())
                throw new ConventionSourceInvalidException("You must supply types to verify");
        }
    }
}