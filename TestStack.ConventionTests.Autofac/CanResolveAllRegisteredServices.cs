namespace TestStack.ConventionTests.Autofac
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Autofac;
    using global::Autofac.Core;

    public class CanResolveAllRegisteredServices : IConvention<AutofacRegistrations>
    {
        private readonly IContainer container;

        public CanResolveAllRegisteredServices(IContainer container)
        {
            this.container = container;
        }

        public void Execute(AutofacRegistrations data, IConventionResultContext result)
        {
            var distinctTypes = data.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<TypedService>().Select(s => s.ServiceType).Union(GetGenericFactoryTypes(data, r)))
                .Distinct();

            var failingTypes = new List<string>();
            foreach (var distinctType in distinctTypes)
            {
                try
                {
                    container.Resolve(distinctType);
                }
                catch (DependencyResolutionException e)
                {
                    failingTypes.Add(e.Message);
                }
            }

            result.Is("Can resolve all types registered with Autofac", failingTypes);
        }

        private IEnumerable<Type> GetGenericFactoryTypes(AutofacRegistrations data, IComponentRegistration componentRegistration)
        {
            return from ctorParameter in data.GetRegistrationCtorParameters(componentRegistration)
                   where ctorParameter.ParameterType.FullName.StartsWith("System.Func")
                   select ctorParameter.ParameterType.GetGenericArguments()[0];
        }
    }
}