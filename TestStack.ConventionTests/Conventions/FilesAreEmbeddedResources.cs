namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class FilesAreEmbeddedResources : IConvention<ProjectFiles>
    {
        public ConventionResult Execute(ProjectFiles data)
        {
            return ConventionResult.For("The following files which should be embedded resources:",
                data.Files.Where(s => s.ReferenceType != "EmbeddedResource"), (t, m) => m.AppendLine("\t" + t.FilePath));
        }
    }
}