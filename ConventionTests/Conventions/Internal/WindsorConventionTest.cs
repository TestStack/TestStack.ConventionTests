namespace ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Handlers;
    using Castle.Windsor.Diagnostics;
    using Castle.Windsor.Diagnostics.Helpers;

    public abstract class WindsorConventionTest<TDiagnostic> : WindsorConventionTest<TDiagnostic, IHandler>
        where TDiagnostic : class, IDiagnostic<IEnumerable<IHandler>>, IDiagnostic<object>
    {
    }

    public abstract class WindsorConventionTest<TDiagnostic, TDiagnosticData> : ConventionTestBase
        where TDiagnostic : class, IDiagnostic<IEnumerable<TDiagnosticData>>, IDiagnostic<object>
    {
        public override void Execute(IAssert assert)
        {
            var conventionData = SetUp();
            var diagnosticData = GetDataToTest(conventionData);
            var itemDescription = (conventionData.ItemDescription ?? (h => h.ToString() + "*"));
            var invalidData = conventionData.Must == null
                                  ? diagnosticData
                                  : Array.FindAll(diagnosticData, h => conventionData.Must(h) == false);
            var message = (conventionData.Description ?? "Invalid elements found") + Environment.NewLine + "\t" +
                          string.Join(Environment.NewLine + "\t", invalidData.Select(itemDescription));
            if (conventionData.HasApprovedExceptions)
            {
                Approve(message);
            }
            else
            {
                assert.AreEqual(0, invalidData.Count(), message);
            }
        }

        protected string MissingDependenciesDescription(IHandler var)
        {
            var message = new StringBuilder("Some dependencies of this component could not be statically resolved.");
            var info = var as IExposeDependencyInfo;
            if (info == null)
            {
                return message.ToString();
            }
            var inspector = new DependencyInspector(message);
            info.ObtainDependencyDetails(inspector);

            return inspector.Message;
        }

        TDiagnosticData[] GetDataToTest(WindsorConventionData<TDiagnosticData> data)
        {
            var host = data.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey) as IDiagnosticsHost;
            IDiagnostic<IEnumerable<TDiagnosticData>> diagnostic = host.GetDiagnostic<TDiagnostic>();
            if (diagnostic == null)
            {
                if (typeof (TDiagnostic).IsInterface)
                {
                    throw new ArgumentException(
                        string.Format("Diagnostic {0} was not found in the container. Did you forget to add it?",
                                      typeof (TDiagnostic)));
                }
                throw new ArgumentException(
                    string.Format(
                        "Diagnostic {0} was not found in the container. The type is not an interface. Did you mean to use one of interfaces it implements instead?",
                        typeof (TDiagnostic)));
            }
            var items = diagnostic.Inspect();
            if (data.OrderBy != null)
            {
                return items.OrderBy(data.OrderBy).ToArray();
            }
            return items.ToArray();
        }

        protected abstract WindsorConventionData<TDiagnosticData> SetUp();
    }

    public abstract class WindsorConventionTest : ConventionTestBase
    {
        public override void Execute(IAssert assert)
        {
            var data = SetUp();
            var handlersToTest = GetHandlersToTest(data);
            if (handlersToTest.Length == 0)
            {
                assert.Inconclusive(
                    "No handlers found to apply the convention to. Make sure the handlesr predicate is correct and that the right components were registered in the container.");
            }
            var itemDescription = (data.ItemDescription ?? (h => h.GetComponentName()));
            var invalidComponents = data.Must == null
                                        ? handlersToTest
                                        : Array.FindAll(handlersToTest, h => data.Must(h) == false);
            var message = (data.Description ?? "Invalid components found") + Environment.NewLine + "\t" +
                          string.Join(Environment.NewLine + "\t", invalidComponents.Select(itemDescription));
            if (data.HasApprovedExceptions)
            {
                Approve(message);
            }
            else
            {
                assert.AreEqual(0, invalidComponents.Count(), message);
            }
        }

        IHandler[] GetHandlersToTest(WindsorConventionData data)
        {
            IHandler[] handlers;
            if (data.Handlers != null)
            {
                handlers = data.Handlers(data.Kernel);
            }
            else
            {
                handlers = data.Kernel.GetAssignableHandlers(typeof (object));
            }
            Array.Sort(handlers,
                       (h1, h2) =>
                       String.Compare(h1.GetComponentName(), h2.GetComponentName(), StringComparison.OrdinalIgnoreCase));

            return handlers;
        }

        protected abstract WindsorConventionData SetUp();
    }
}