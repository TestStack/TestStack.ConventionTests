namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class ApiControllerNamingAndBaseClassConvention : MvcControllerNameAndBaseClassConvention
    {
        protected override IEnumerable<Type> GetControllers(Types data)
        {
            return data.TypesToVerify.Where(IsWebApiController);
        }

        protected override string GetControllerTypeName()
        {
            return "Api Controller";
        }
    }
}