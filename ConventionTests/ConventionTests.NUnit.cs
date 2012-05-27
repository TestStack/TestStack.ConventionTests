using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ApprovalTests;
using ApprovalTests.Core;
using ApprovalTests.Namers;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using Castle.Windsor.Diagnostics.DebuggerViews;
using Castle.Windsor.Diagnostics.Helpers;
using NUnit.Framework;
using Approvals = ApprovalTests.Approvals;

namespace ConventionTests
{
	public interface IConventionTest
	{
		string Name { get; }
		void Execute();
	}


	/// <summary>
	///   This is where we set what our convention is all about
	/// </summary>
	public class ConventionData
	{
		/// <summary>
		///   list of assemblies to scan for types that our convention is related to. Can be null, in which case all assemblies starting with 'Als.' will be scanned
		/// </summary>
		public Assembly[] Assemblies { get; set; }

		/// <summary>
		///   Descriptive text used for failure message in test. Should explan what is wrong, and how to fix it (how to make types that do not conform to the convention do so).
		/// </summary>
		public string FailDescription { get; set; }

		/// <summary>
		///   Specifies that there are valid exceptions to the rule specified by the convention.
		/// </summary>
		/// <remarks>
		///   When set to <c>true</c> will run the test as Approval test so that the exceptional cases can be reviewed and approved.
		/// </remarks>
		public bool HasApprovedExceptions { get; set; }

		/// <summary>
		///   This is the convention. The predicate should return <c>true</c> for types that do conform to the convention, and <c>false</c> otherwise
		/// </summary>
		public Predicate<Type> Must { get; set; }

		/// <summary>
		///   Predicate that finds types that we want to apply out convention to.
		/// </summary>
		public Predicate<Type> Types { get; set; }

		public Func<Type, string> FailItemDescription { get; set; }

		/// <summary>
		///   helper method to set <see cref="Assemblies" /> in a more convenient manner.
		/// </summary>
		/// <param name="assembly"> </param>
		/// <returns> </returns>
		public ConventionData FromAssembly(params Assembly[] assembly)
		{
			Assemblies = assembly;
			return this;
		}

		/// <summary>
		///   helper method to set <see cref="HasApprovedExceptions" /> in a more convenient manner.
		/// </summary>
		/// <returns> </returns>
		public ConventionData WithApprovedExceptions(string explanation = null)
		{
			HasApprovedExceptions = true;
			return this;
		}
	}

	public abstract class ConventionTestBase : IConventionTest
	{
		public virtual string Name
		{
			get { return GetType().Name.Replace('_', ' '); }
		}

		public abstract void Execute();

		protected void Approve(string message)
		{
			Approvals.Verify(new ApprovalTextWriter(message), new ConventionTestNamer(GetType().Name),
			                 Approvals.GetReporter());
		}
	}

	/// <summary>
	///   Base class for convention tests. Inherited types should be put in "/Conventions" folder in test assembly and follow Sentence_naming_convention_with_underscores_indead_of_spaces These tests will be ran by <see
	///    cref="ConventionTestsRunner" /> .
	/// </summary>
	public abstract class ConventionTest : ConventionTestBase
	{
		public override void Execute()
		{
			var data = SetUp();
			var typesToTest = GetTypesToTest(data);
			if (typesToTest.Length == 0)
			{
				Assert.Inconclusive(
					"No types found to apply the convention to. Make sure the Types predicate is correct and that the right assemblies to scan are specified.");
			}
			var itemDescription = (data.FailItemDescription ?? (t => t.ToString()));
			var invalidTypes = Array.FindAll(typesToTest, t => data.Must(t) == false);
			var message = (data.FailDescription ?? "Invalid types found") + Environment.NewLine + "\t" +
			              string.Join(Environment.NewLine + "\t", invalidTypes.Select(itemDescription));
			if (data.HasApprovedExceptions)
			{
				Approve(message);
			}
			else
			{
				Assert.AreEqual(0, invalidTypes.Count(), message);
			}
		}

		protected virtual Assembly[] GetAssembliesToScan(ConventionData data)
		{
			if (data.Assemblies != null)
			{
				return data.Assemblies;
			}
			var assembly = Assembly.GetCallingAssembly();
			var companyName = assembly.FullName.Substring(0, assembly.FullName.IndexOf('.'));
			var assemblyNames = assembly.GetReferencedAssemblies();
			var applicationAssemblies = Array.FindAll(assemblyNames, n => n.FullName.StartsWith(companyName));
			return Array.ConvertAll(applicationAssemblies, Assembly.Load);
		}

		/// <summary>
		///   This is the only method you need to override. Return a <see cref="ConventionData" /> that describes your convention.
		/// </summary>
		/// <returns> </returns>
		protected abstract ConventionData SetUp();

		protected virtual Type[] GetTypesToTest(ConventionData data)
		{
			return
				GetAssembliesToScan(data).SelectMany(a => a.GetTypes()).Where(data.Types.Invoke).OrderBy(t => t.FullName)
					.ToArray();
		}
	}

	public abstract class WindsorConventionTest<TDiagnostic> : WindsorConventionTest<TDiagnostic, IHandler>
		where TDiagnostic : class, IDiagnostic<IEnumerable<IHandler>>, IDiagnostic<object>
	{
	}

	public abstract class WindsorConventionTest<TDiagnostic, TDiagnosticData> : ConventionTestBase
		where TDiagnostic : class, IDiagnostic<IEnumerable<TDiagnosticData>>, IDiagnostic<object>
	{
		public override void Execute()
		{
			var conventionData = SetUp();
			var diagnosticData = GetDataToTest(conventionData);
			var itemDescription = (conventionData.FailItemDescription ?? (h => h.ToString() + "*"));
			var invalidData = conventionData.Must == null
			                  	? diagnosticData
			                  	: Array.FindAll(diagnosticData, h => conventionData.Must(h) == false);
			var message = (conventionData.FailDescription ?? "Invalid elements found") + Environment.NewLine + "\t" +
			              string.Join(Environment.NewLine + "\t", invalidData.Select(itemDescription));
			if (conventionData.HasApprovedExceptions)
			{
				Approve(message);
			}
			else
			{
				Assert.AreEqual(0, invalidData.Count(), message);
			}
		}

		protected string MissingDependenciesDescription(IHandler var)
		{
			var item = new ComponentStatusDebuggerViewItem((IExposeDependencyInfo) var);
			return item.Message;
		}

		private TDiagnosticData[] GetDataToTest(WindsorConventionData<TDiagnosticData> data)
		{
			var host = data.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey) as IDiagnosticsHost;
			IDiagnostic<IEnumerable<TDiagnosticData>> diagnostic = host.GetDiagnostic<TDiagnostic>();
			if (diagnostic == null)
			{
				if (typeof (TDiagnostic).IsInterface)
				{
					throw new ArgumentException(
						string.Format("Diagnostic {0} was not found in the container. Did you forget to add it?", typeof (TDiagnostic)));
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
		public override void Execute()
		{
			var data = SetUp();
			var handlersToTest = GetHandlersToTest(data);
			if (handlersToTest.Length == 0)
			{
				Assert.Inconclusive(
					"No handlers found to apply the convention to. Make sure the handlesr predicate is correct and that the right components were registered in the container.");
			}
			var itemDescription = (data.FailItemDescription ?? (h => h.GetComponentName()));
			var invalidComponents = data.Must == null
			                        	? handlersToTest
			                        	: Array.FindAll(handlersToTest, h => data.Must(h) == false);
			var message = (data.FailDescription ?? "Invalid components found") + Environment.NewLine + "\t" +
			              string.Join(Environment.NewLine + "\t", invalidComponents.Select(itemDescription));
			if (data.HasApprovedExceptions)
			{
				Approve(message);
			}
			else
			{
				Assert.AreEqual(0, invalidComponents.Count(), message);
			}
		}

		private IHandler[] GetHandlersToTest(WindsorConventionData data)
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

	public class WindsorConventionData<TDiagnosticData>
	{
		private readonly IWindsorContainer container;

		public WindsorConventionData(IWindsorContainer container)
		{
			this.container = container;
		}

		public IKernel Kernel
		{
			get { return Container.Kernel; }
		}

		public IWindsorContainer Container
		{
			get { return container; }
		}

		public Func<TDiagnosticData, string> FailItemDescription { get; set; }

		public Func<TDiagnosticData, bool> Must { get; set; }

		public string FailDescription { get; set; }

		public bool HasApprovedExceptions { get; set; }

		public Func<TDiagnosticData, object> OrderBy { get; set; }
	}

	public class WindsorConventionData
	{
		private readonly IWindsorContainer container;

		public WindsorConventionData(IWindsorContainer container)
		{
			this.container = container;
		}

		public Func<IKernel, IHandler[]> Handlers { get; set; }

		public Func<IHandler, bool> Must { get; set; }

		public IKernel Kernel
		{
			get { return Container.Kernel; }
		}

		public IWindsorContainer Container
		{
			get { return container; }
		}

		public Func<IHandler, string> FailItemDescription { get; set; }

		public string FailDescription { get; set; }

		public bool HasApprovedExceptions { get; set; }
	}

	public class ConventionTestNamer : UnitTestFrameworkNamer, IApprovalNamer
	{
		private readonly string name;

		public ConventionTestNamer(string name)
		{
			this.name = name;
		}

		string IApprovalNamer.Name
		{
			get { return name; }
		}
	}
}