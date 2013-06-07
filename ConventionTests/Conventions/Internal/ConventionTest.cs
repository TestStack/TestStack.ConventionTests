namespace ConventionTests
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Base class for convention tests. Inherited types should be put in "/Conventions" folder in test assembly and follow Sentence_naming_convention_with_underscores_indead_of_spaces These tests will be ran by
    ///     <see
    ///         cref="ConventionTestsRunner" />
    ///     .
    /// </summary>
    public abstract class ConventionTest : ConventionTestBase
    {
        public override void Execute(IAssert assert)
        {
            var data = SetUp();
            var typesToTest = GetTypesToTest(data);
            if (typesToTest.Length == 0)
            {
                assert.Inconclusive(
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
                assert.AreEqual(0, invalidTypes.Count(), message);
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
            var assemblies = Array.ConvertAll(applicationAssemblies, n => n.TryLoadAssembly());
            return Array.FindAll(assemblies, a => a != null);
        }

        /// <summary>
        ///     This is the only method you need to override. Return a <see cref="ConventionData" /> that describes your convention.
        /// </summary>
        /// <returns> </returns>
        protected abstract ConventionData SetUp();

        protected virtual Type[] GetTypesToTest(ConventionData data)
        {
            if (data.SourceTypes != null)
            {
                return data.SourceTypes;
            }

            return
                GetAssembliesToScan(data).SelectMany(a => a.SafeGetTypes()).Where(data.Types.Invoke).OrderBy(
                    t => t.FullName)
                                         .ToArray();
        }
    }
}