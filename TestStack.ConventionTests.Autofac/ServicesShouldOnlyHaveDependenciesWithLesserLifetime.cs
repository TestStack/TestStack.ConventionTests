namespace TestStack.ConventionTests.Autofac
{
    using global::Autofac.Core;
    using System.Collections.Generic;
    using TestStack.ConventionTests.ConventionData;

    public class ServicesShouldOnlyHaveDependenciesWithLesserLifetime : IConvention<AutofacRegistrations>
    {
        public void Execute(AutofacRegistrations data, IConventionResultContext result)
        {
            var exceptions = new List<string>();
            foreach (var registration in data.ComponentRegistry.Registrations)
            {
                var registrationLifetime = data.GetLifetime(registration);

                foreach (var ctorParameter in data.GetRegistrationCtorParameters(registration))
                {
                    IComponentRegistration parameterRegistration;
                    var typedService = new TypedService(ctorParameter.ParameterType);

                    // If the parameter is not registered with autofac, ignore
                    if (!data.ComponentRegistry.TryGetRegistration(typedService, out parameterRegistration)) continue;

                    var parameterLifetime = data.GetLifetime(parameterRegistration);

                    if (parameterLifetime >= registrationLifetime) continue;

                    var typeName = data.GetConcreteType(registration).ToTypeNameString();
                    var parameterType = ctorParameter.ParameterType.ToTypeNameString();

                    var error = string.Format("{0} ({1}) => {2} ({3})",
                        typeName, registrationLifetime,
                        parameterType, parameterLifetime);
                    exceptions.Add(error);
                }
            }

            result.Is("Components should not depend on services with lesser lifetimes", exceptions);
        }

        public string ConventionReason
        {
            get { return @"When classes with larger lifetimes depend on classes with smaller lifetimes this often indicates an error and can lead to subtle bugs"; }
        }
    }
}