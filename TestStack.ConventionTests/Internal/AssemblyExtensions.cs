using System;
using System.Reflection;

namespace TestStack.ConventionTests.Internal
{
    internal static class AssemblyExtensions {

        public static string TryGetExecutingAssembly(this Assembly assembly) =>
        #if NewReflection
        Assembly.GetEntryAssembly().CodeBase;
        #else
        Assembly.GetExecutingAssembly().CodeBase;
        #endif

    }
}