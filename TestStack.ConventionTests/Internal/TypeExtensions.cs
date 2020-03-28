namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Reflection;

    internal static class TypeExtensions
    {
        public static bool IsGenericType(this Type type) =>
        #if NewReflection
            type.GetTypeInfo().IsGenericType;
        #else
            type.IsGenericType;
        #endif
        
        public static bool IsDefined(this Type type, Type attributeType, bool inherit) => 
        #if NewReflection
            type.GetTypeInfo().IsDefined(attributeType, inherit);
        #else
            type.IsDefined(attributeType, true);
        #endif

        public static Assembly GetAssembly(this Type type) =>
        #if NewReflection
            type.GetTypeInfo().Assembly;
        #else
            type.Assembly;
        #endif
        
        public static Type GetBaseType(this Type type) =>
        #if NewReflection
            type.GetTypeInfo().BaseType;
        #else
            type.BaseType;
        #endif
    }
}