﻿namespace TestStack.ConventionTests.Conventions
{
    using System;
    using TestStack.ConventionTests.ConventionData;

    /// <summary>
    ///     This convention allows you to enforce a particular type of class is under a namespace, for instance.
    ///     Dto must be under App.Contracts.Dtos
    ///     Domain Objects must be under App.Domain
    ///     Event Handlers must be under App.Handlers
    ///     This is a Symmetric convention, and will verify all of a Class Type lives in the namespace, but also that only that
    ///     class type is in that namespace
    /// </summary>
    public class ClassTypeHasSpecificNamespace : IConvention<Types>
    {
        readonly Func<Type, bool> classIsApplicable;
        readonly string classType;
        readonly string namespaceToCheck;

        /// <summary>
        ///     Ctor
        /// </summary>
        /// <param name="classIsApplicable">Predicate to verify if the class is the right type</param>
        /// <param name="namespaceToCheck"></param>
        /// <param name="classType">The class type. ie, Dto, Domain Object, Event Handler</param>
        public ClassTypeHasSpecificNamespace(Func<Type, bool> classIsApplicable, string namespaceToCheck,
            string classType)
        {
            this.classIsApplicable = classIsApplicable;
            this.namespaceToCheck = namespaceToCheck;
            this.classType = classType;
        }

        public string InverseTitle
        {
            get { return string.Format("Non-{0}s must not be under the '{1}' namespace", classType, namespaceToCheck); }
        }

        public string ConventionTitle
        {
            get { return string.Format("{0}s must be under the '{1}' namespace", classType, namespaceToCheck); }
        }

        public void Execute(Types data, IConventionResult result)
        {
            result.IsSymmetric(data.TypesToVerify,
                classIsApplicable,
                t => t.Namespace != null && t.Namespace.StartsWith(namespaceToCheck),
                ConventionTitle,
                InverseTitle);
        }
    }
}