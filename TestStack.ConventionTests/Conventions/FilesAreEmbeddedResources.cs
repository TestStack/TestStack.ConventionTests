namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class FilesAreEmbeddedResources : IConvention<ProjectFiles>
    {
        public FilesAreEmbeddedResources(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        public string FileExtension { get; set; }

        public string ConventionTitle
        {
            get { return string.Format("{0} Files must be embedded resources", FileExtension); }
        }

        public ConventionResult Execute(ProjectFiles data)
        {
            return
                ConventionResult.For(
                    data.Files.Where(s => s.FilePath.EndsWith(FileExtension) && s.ReferenceType != "EmbeddedResource"));
        }
    }
}