namespace TestStack.ConventionTests.ConventionData
{
    using System;
    using System.Linq;

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

        public bool HasData {get { return TypesToVerify.Any(); }}

        public static Types InAssemblyOf<T>()
        {
            var assembly = typeof (T).Assembly;
            return new Types(assembly.GetName().Name)
            {
                TypesToVerify = assembly.GetTypes()
            };
        }
    }
}