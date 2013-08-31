namespace TestStack.ConventionTests.Conventions
{
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class FilesAreEmbeddedResources : IConvention<ProjectFileItems>
    {
        public FilesAreEmbeddedResources(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        public string FileExtension { get; private set; }

        public void Execute(ProjectFileItems data, IConventionResultContext result)
        {
            result.Is(
                string.Format("{0} Files must be embedded resources", FileExtension),
                data.Items.Where(s => s.FilePath.EndsWith(FileExtension) && s.ReferenceType != "EmbeddedResource"));
        }
    }
}