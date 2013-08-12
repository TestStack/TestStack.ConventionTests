namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class ApiControllerConvention : IConvention<Types>
    {
        public void Execute(Types data, IConventionResult result)
        {
            var controllers = data.TypesToVerify.Where(IsWebApiController);
            var typesWhichDoNotEndInController = controllers.Where(c => !c.Name.EndsWith("Controller"));

            var typesWhichEndInController = data.TypesToVerify.Where(t => t.Name.EndsWith("Controller"));
            var controllersWhichDoNotInheritFromController =
                typesWhichEndInController.Where(t => !IsMvcController(t) && !IsWebApiController(t));

            result.IsSymmetric(
                "WebApi controllers must be suffixed with Controller", typesWhichDoNotEndInController,
                "Types named *Controller must inherit from ApiController or Controller",
                controllersWhichDoNotInheritFromController);
        }

        static bool IsMvcController(Type arg)
        {
            var isController = arg.FullName == "System.Web.Mvc.Controller";
            if (arg.BaseType == null)
                return isController;
            return isController || IsMvcController(arg.BaseType);
        }

        static bool IsWebApiController(Type arg)
        {
            var isController = arg.FullName == "System.Web.Http.ApiController";
            if (arg.BaseType == null)
                return isController;
            return isController || IsWebApiController(arg.BaseType);
        }
    }
}