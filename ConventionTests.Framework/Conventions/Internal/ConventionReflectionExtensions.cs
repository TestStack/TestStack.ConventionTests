namespace ConventionTests
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class ConventionReflectionExtensions
    {
        public static IConventionTest[] GetAllConventions(Assembly assembly)
        {
            var conventionTypes = GetConventionTypes();
            return Array.ConvertAll(conventionTypes, CreateConvention);
        }

        static bool IsConventionTest(Type type)
        {
            return type.IsClass && type.IsAbstract == false && typeof(IConventionTest).IsAssignableFrom(type);
        }

        static IConventionTest CreateConvention(Type t)
        {
            return (IConventionTest)Activator.CreateInstance(t);
        }

        static Type[] GetConventionTypes()
        {
            var types =
                Assembly.GetExecutingAssembly().GetExportedTypes().Where(
                    IsConventionTest).ToArray();
            return types;
        } 
    }
}