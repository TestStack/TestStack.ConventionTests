namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Linq;

    /// <summary>
    ///     This is where we set what our convention is all about.
    /// </summary>
    public class Types : IConventionData
    {
        //NOTE: that's a terrible name
        public Type[] ApplicableTypes { get; set; }

        public bool HasApprovedExceptions { get; set; }

        public bool HasValidSource
        {
            get { return ApplicableTypes.Any(); }
        }
    }
}