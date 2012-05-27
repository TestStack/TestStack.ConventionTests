using System;
using System.Linq;
using System.Reflection;
using ApprovalTests;
using ApprovalTests.Core;
using ApprovalTests.Namers;
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

	/// <summary>
	///   Base class for convention tests. Inherited types should be put in "/Conventions" folder in test assembly and follow Sentence_naming_convention_with_underscores_indead_of_spaces These tests will be ran by <see
	///    cref="ConventionTestsRunner" /> .
	/// </summary>
	public abstract class ConventionTest : IConventionTest
	{
		#region IConventionTest Members

		public virtual string Name
		{
			get { return GetType().Name.Replace('_', ' '); }
		}

		public virtual void Execute()
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

		#endregion

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

		private void Approve(string message)
		{
			Approvals.Verify(new ApprovalTextWriter(message), new ConventionTestNamer(GetType().Name),
			                 Approvals.GetReporter());
		}

		protected virtual Type[] GetTypesToTest(ConventionData data)
		{
			return
				GetAssembliesToScan(data).SelectMany(a => a.GetTypes()).Where(data.Types.Invoke).OrderBy(t => t.FullName)
					.ToArray();
		}
	}

	public class ConventionTestNamer : UnitTestFrameworkNamer, IApprovalNamer
	{
		private readonly string name;

		public ConventionTestNamer(string name)
		{
			this.name = name;
		}

		#region IApprovalNamer Members

		string IApprovalNamer.Name
		{
			get { return name; }
		}

		#endregion
	}
}