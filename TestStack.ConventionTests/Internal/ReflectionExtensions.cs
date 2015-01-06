namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
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
    }
}