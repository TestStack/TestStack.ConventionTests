namespace TestStack.ConventionTests.ConventionData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class TypeExtensions
    {
        public static bool IsConcreteClass(this Type t)
        {
            return t.IsClass && !t.IsAbstract;
        }

        public static bool IsEnum(this Type type)
        {
            return typeof(Enum).IsAssignableFrom(type);
        }

        public static bool IsStatic(this Type type)
        {
            return type.IsClass && !(type.IsSealed && type.IsAbstract);
        }

        public static bool HasDefaultConstructor(this Type type)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic |
                                        BindingFlags.DeclaredOnly)
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

        public static IEnumerable<MethodInfo> NonVirtualMethods(this Type type)
        {
            var methodInfos =
                type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic |
                                BindingFlags.DeclaredOnly);
            return methodInfos.Where(methodInfo =>
                !methodInfo.IsPrivate &&
                methodInfo.DeclaringType == type &&
                !methodInfo.Name.StartsWith("<"))
                .Where(methodInfo => methodInfo.Name != "Equals")
                .Where(methodInfo => !methodInfo.IsVirtual || methodInfo.IsFinal);
        }

        public static IEnumerable<Type> GetClosedInterfacesOf(this Type type, Type openGeneric)
        {
            return from i in type.GetInterfaces()
                   where i.IsGenericType
                   let defn = i.GetGenericTypeDefinition()
                   where defn == openGeneric
                   select i;
        }

        public static bool ClosesInterface(this Type t, Type openGeneric)
        {
            return t.GetClosedInterfacesOf(openGeneric).Any();
        }

        internal static string GetSentenceCaseName(this Type type)
        {
            return Regex.Replace(type.Name, "[a-z][A-Z]", m => m.Value[0] + " " + Char.ToLower(m.Value[1]));
        }

        /// <summary>
        /// Type.ToString does not read very well for generics, use this method to print in a nice way
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToTypeNameString(this Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                return nullableType.Name + "?";

            if (!type.IsGenericType)
                switch (type.Name)
                {
                    case "String":
                        return "string";
                    case "Int32":
                        return "int";
                    case "Decimal":
                        return "decimal";
                    case "Object":
                        return "object";
                    case "Void":
                        return "void";
                    default:
                    {
                        return String.IsNullOrWhiteSpace(type.FullName) ? type.Name : type.FullName;
                    }
                }

            var sb = new StringBuilder(type.Name.Substring(0,
                type.Name.IndexOf('`'))
                );
            sb.Append('<');
            var first = true;
            foreach (var t in type.GetGenericArguments())
            {
                if (!first)
                    sb.Append(',');
                sb.Append(t.ToTypeNameString());
                first = false;
            }
            sb.Append('>');
            return sb.ToString();
        }
    }
}