namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class FilesAreEmbeddedResources : IConvention<ProjectFiles>
    {
        public ConventionResult Execute(ProjectFiles data)
        {
            return ConventionResult.For(data.Files.Where(s => s.ReferenceType != "EmbeddedResource"),
                "The following files which should be embedded resources:",
                (t, m) => m.AppendLine("\t" + t.FilePath));
        }
    }
}