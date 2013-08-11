namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class FilesAreEmbeddedResources : IConvention<ProjectFiles>
    {
        public FilesAreEmbeddedResources(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        public string FileExtension { get; private set; }

        public void Execute(ProjectFiles data, IConventionResult result)
        {
            result.Is(
                string.Format("{0} Files must be embedded resources", FileExtension),
                data.Files.Where(s => s.FilePath.EndsWith(FileExtension) && s.ReferenceType != "EmbeddedResource"));
        }
    }
}