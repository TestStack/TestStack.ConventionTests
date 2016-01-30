namespace TestStack.ConventionTests.Internal
{
    using System.IO;
    using System.Xml.Linq;

    public class ProjectFileFromDiskProvider : IProjectProvider
    {
        readonly string projectFilePath;

        public ProjectFileFromDiskProvider(string projectFilePath)
        {
            this.projectFilePath = projectFilePath;
        }

        public XDocument LoadProjectDocument()
        {
            return XDocument.Load(projectFilePath);
        }

        public string GetName()
        {
            return Path.GetFileNameWithoutExtension(projectFilePath);
        }
    }
}