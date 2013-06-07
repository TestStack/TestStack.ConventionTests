namespace ConventionTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static Assembly TryLoadAssembly(this AssemblyName name)
        {
            try
            {
                return Assembly.Load(name);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (FileLoadException)
            {
                return null;
            }
            catch (BadImageFormatException)
            {
                return null;
            }
            catch (ReflectionTypeLoadException)
            {
                return null;
            }
        }

        public static Type[] SafeGetTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return Array.FindAll(ex.Types, x => x != null);
            }
        }

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