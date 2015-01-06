namespace TestStack.ConventionTests.ConventionData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

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

        public static Types InAssemblyOf<T>(bool excludeCompilerGeneratedTypes = true)
        {
            var assembly = typeof(T).Assembly;
            var typesToVerify = assembly.GetTypes();
            if (excludeCompilerGeneratedTypes)
            {
                typesToVerify = typesToVerify
                    .Where(t => !t.GetCustomAttributes(typeof (CompilerGeneratedAttribute), true).Any())
                    .ToArray();
            }
            return new Types(assembly.GetName().Name)
            {
                TypesToVerify = typesToVerify
            };
        }

        public static Types InAssemblyOf<T>(string descriptionOfTypes, Func<IEnumerable<Type>, IEnumerable<Type>> types)
        {
            var assembly = typeof (T).Assembly;
            return new Types(descriptionOfTypes)
            {
                TypesToVerify = types(assembly.GetTypes()).ToArray()
            };
        }
    }
}