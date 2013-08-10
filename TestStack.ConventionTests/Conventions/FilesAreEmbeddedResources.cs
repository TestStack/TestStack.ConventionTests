namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class FilesAreEmbeddedResources : IConvention<ProjectFiles>
    {
        public FilesAreEmbeddedResources(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        public string ConventionTitle
        {
            get
            {
                return string.Format("{0} Files must be embedded resources", FileExtension);
            }
        }

        public string FileExtension { get; set; }

        public IEnumerable<object> GetFailingData(ProjectFiles data)
        {
            return data.Files.Where(s => s.FilePath.EndsWith(FileExtension) && s.ReferenceType != "EmbeddedResource");
        }
    }
}