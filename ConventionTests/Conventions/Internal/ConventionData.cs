namespace ConventionTests
{
    using System;
    using System.Reflection;

    /// <summary>
    ///     This is where we set what our convention is all about
    /// </summary>
    public class ConventionData
    {
        public static readonly Predicate<Type> All = _ => true;
        public static readonly Predicate<Type> Fail = _ => false;

        public ConventionData() : this(null)
        {
        }

        /// <summary>
        ///     List of types to apply the convention to. Mutually exclusive with <see cref="Assemblies" /> property. If
        ///     <paramref
        ///         name="sourceTypes" />
        ///     is specified <see cref="Assemblies" /> property will be ignored.
        /// </summary>
        /// <param name="sourceTypes"> </param>
        public ConventionData(params Type[] sourceTypes)
        {
            SourceTypes = sourceTypes;
            Types = All;
            Must = Fail;
        }

        public Type[] SourceTypes { get; set; }


        /// <summary>
        ///     list of assemblies to scan for types that our convention is related to. Can be null, in which case all assemblies starting with The same prefix as your test assembly will be scanned
        /// </summary>
        public Assembly[] Assemblies { get; set; }

        /// <summary>
        ///     Descriptive text used for failure message in test. Should explan what is wrong, and how to fix it (how to make types that do not conform to the convention do so).
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Specifies that there are valid exceptions to the rule specified by the convention.
        /// </summary>
        /// <remarks>
        ///     When set to <c>true</c> will run the test as Approval test so that the exceptional cases can be reviewed and approved.
        /// </remarks>
        public bool HasApprovedExceptions { get; set; }

        /// <summary>
        ///     This is the convention. The predicate should return <c>true</c> for types that do conform to the convention, and <c>false</c> otherwise
        /// </summary>
        public Predicate<Type> Must { get; set; }

        /// <summary>
        ///     Predicate that finds types that we want to apply out convention to.
        /// </summary>
        public Predicate<Type> Types { get; set; }

        public Func<Type, string> ItemDescription { get; set; }

        /// <summary>
        ///     helper method to set <see cref="Assemblies" /> in a more convenient manner.
        /// </summary>
        /// <param name="assembly"> </param>
        /// <returns> </returns>
        public ConventionData FromAssembly(params Assembly[] assembly)
        {
            Assemblies = assembly;
            return this;
        }

        /// <summary>
        ///     helper method to set <see cref="HasApprovedExceptions" /> in a more convenient manner.
        /// </summary>
        /// <returns> </returns>
        public ConventionData WithApprovedExceptions(string explanation = null)
        {
            HasApprovedExceptions = true;
            return this;
        }
    }
}