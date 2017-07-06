namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TestStack.ConventionTests.ConventionData;

    public class MvcControllerNameAndBaseClassConvention : IConvention<Types>
    {
        public void Execute(Types data, IConventionResultContext result)
        {
            var controllers = GetControllers(data);
            var typesWhichDoNotEndInController = controllers.Where(c => !c.Name.EndsWith("Controller"));

            var typesWhichEndInController = data.TypesToVerify.Where(t => t.Name.EndsWith("Controller"));
            var controllersWhichDoNotInheritFromController =
                typesWhichEndInController.Where(t => !IsMvcController(t) && !IsWebApiController(t));

            result.IsSymmetric(
                GetControllerTypeName() + "s must be suffixed with Controller", typesWhichDoNotEndInController,
                "Types named *Controller must inherit from ApiController or Controller",
                controllersWhichDoNotInheritFromController);
        }

        public string ConventionReason { get { return "This convention detects when Mvc Controllers are likely misconfigured and do not follow Mvc conventions"; } }

        protected virtual string GetControllerTypeName()
        {
            return "Mvc Controller";
        }

        protected virtual IEnumerable<Type> GetControllers(Types data)
        {
            return data.TypesToVerify.Where(IsMvcController);
        }

        static bool IsMvcController(Type arg)
        {
            var isController = arg.FullName == "System.Web.Mvc.Controller";
            var baseType = arg.GetTypeInfo().BaseType;
            if (baseType == null)
                return isController;
            return isController || IsMvcController(baseType);
        }

        protected static bool IsWebApiController(Type arg)
        {
            var isController = arg.FullName == "System.Web.Http.ApiController";
            var baseType = arg.GetTypeInfo().BaseType;
            if (baseType == null)
                return isController;
            return isController || IsWebApiController(baseType);
        }
    }
}