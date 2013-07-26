namespace TestStack.ConventionTests.Helpers
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

        public static bool IsEnum(this Type type)
        {
            return typeof(Enum).IsAssignableFrom(type);
        }

        public static bool HasAttribute<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), inherit).Length > 0;
        }

        public static bool IsStatic(this Type type)
        {
            return type.IsClass && !(type.IsSealed && type.IsAbstract);
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Any(constructorInfo => constructorInfo.GetParameters().Length == 0 && !constructorInfo.IsPrivate);
        }

        public static bool HasPublicDefaultConstructor(this Type type)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Any(constructorInfo => constructorInfo.GetParameters().Length == 0);
        }

        public static bool AssignableTo<TAssignableTo>(this Type type)
        {
            return typeof(TAssignableTo).IsAssignableFrom(type);
        }

        public static IEnumerable<Type> ConcreteTypes(this IEnumerable<Type> types)
        {
            return types.Where(t => t.IsClass && !t.IsAbstract);
        }

        public static IEnumerable<MethodInfo> NonVirtualMethods(this Type type)
        {
            var methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            return methodInfos
                .Where(methodInfo => !methodInfo.IsPrivate && methodInfo.DeclaringType == type && !methodInfo.Name.StartsWith("<"))
                .Where(methodInfo => methodInfo.Name != "Equals")
                .Where(methodInfo => !methodInfo.IsVirtual || methodInfo.IsFinal);
        }
    }
}