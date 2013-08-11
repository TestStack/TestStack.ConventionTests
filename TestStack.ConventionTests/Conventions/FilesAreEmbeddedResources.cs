namespace TestStack.ConventionTests.Conventions
{
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public class FilesAreEmbeddedResources : Convention<ProjectFiles>
    {
        public FilesAreEmbeddedResources(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        public string FileExtension { get; set; }

        public override string ConventionTitle
        {
            get { return string.Format("{0} Files must be embedded resources", FileExtension); }
        }

        protected override IEnumerable<object> Execute(ProjectFiles data)
        {
            return data.Files.Where(s => s.FilePath.EndsWith(FileExtension) && s.ReferenceType != "EmbeddedResource");
        }
    }
}