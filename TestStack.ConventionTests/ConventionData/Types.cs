namespace TestStack.ConventionTests.ConventionData
{
    using System;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Internal;

    /// <summary>
    ///     This is where we set what our convention is all about.
    /// </summary>
    public class Types : IConventionData
    {
        public Types(string descriptionOfTypes)
        {
            Description = descriptionOfTypes;
        }

        public Type[] TypesToVerify { get; set; }

        public string Description { get; private set; }

        public void EnsureHasNonEmptySource()
        {
            if (TypesToVerify.None())
                throw new ConventionSourceInvalidException("You must supply types to verify");
        }
    }
}