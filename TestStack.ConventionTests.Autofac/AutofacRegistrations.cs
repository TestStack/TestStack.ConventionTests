namespace TestStack.ConventionTests.Autofac
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::Autofac.Core;
    using global::Autofac.Core.Activators.Delegate;
    using global::Autofac.Core.Activators.ProvidedInstance;
    using global::Autofac.Core.Activators.Reflection;
    using global::Autofac.Core.Lifetime;

    public class AutofacRegistrations : IConventionData
    {
        private readonly IComponentRegistry componentRegistry;

        public AutofacRegistrations(IComponentRegistry componentRegistry)
        {
            this.componentRegistry = componentRegistry;
        }

        public string Description
        {
            get { return "All AutofacContainer Registrations"; }
        }

        public bool HasData
        {
            get { return true; }
        }

        public IComponentRegistry ComponentRegistry
        {
            get { return componentRegistry; }
        }

        public Type GetConcreteType(IComponentRegistration r)
        {
            var reflectionActivator = r.Activator as ReflectionActivator;
            if (reflectionActivator != null) return reflectionActivator.LimitType;

            var delegateActivator = r.Activator as DelegateActivator;
            if (delegateActivator != null) return delegateActivator.LimitType;

            var providedInstanceActivator = r.Activator as ProvidedInstanceActivator;
            if (providedInstanceActivator != null) return providedInstanceActivator.LimitType;

            throw new InvalidOperationException(r.Activator.GetType() + " is not a known component registration type");
        }

        public Lifetime GetLifetime(IComponentRegistration componentRegistration)
        {
            if (componentRegistration.Ownership == InstanceOwnership.OwnedByLifetimeScope && componentRegistration.Sharing == InstanceSharing.Shared &&
                componentRegistration.Lifetime is RootScopeLifetime)
                return Lifetime.SingleInstance;
            if (componentRegistration.Ownership == InstanceOwnership.OwnedByLifetimeScope && componentRegistration.Sharing == InstanceSharing.Shared &&
                componentRegistration.Lifetime is CurrentScopeLifetime)
                return Lifetime.InstancePerLifetimeScope;
            if (componentRegistration.Ownership == InstanceOwnership.OwnedByLifetimeScope && componentRegistration.Sharing == InstanceSharing.None &&
                componentRegistration.Lifetime is CurrentScopeLifetime)
                return Lifetime.Transient;
            if (componentRegistration.Ownership == InstanceOwnership.ExternallyOwned && componentRegistration.Sharing == InstanceSharing.None &&
                componentRegistration.Lifetime is CurrentScopeLifetime)
                return Lifetime.ExternallyOwned;
            if (componentRegistration.Ownership == InstanceOwnership.ExternallyOwned && componentRegistration.Sharing == InstanceSharing.Shared &&
                componentRegistration.Lifetime is CurrentScopeLifetime)
                return Lifetime.SingleInstanceExternallyOwned;

            throw new InvalidOperationException(string.Format("Unknown registration type for {3} Ownership: {0}, Sharing: {1}, Lifetime type: {2}", componentRegistration.Ownership, componentRegistration.Sharing,
                componentRegistration.Lifetime.GetType().Name, GetConcreteType(componentRegistration)));
        }

        public IEnumerable<ParameterInfo> GetRegistrationCtorParameters(IComponentRegistration componentRegistration)
        {
            var activator = componentRegistration.Activator as ReflectionActivator;
            if (activator == null)
                return Enumerable.Empty<ParameterInfo>();

            var limitType = activator.LimitType;
            return activator.ConstructorFinder.FindConstructors(limitType).SelectMany(ctor => ctor.GetParameters());
        }
    }
}