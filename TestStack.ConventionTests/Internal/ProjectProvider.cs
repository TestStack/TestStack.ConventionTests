namespace TestStack.ConventionTests.Internal
{
    using System.Xml.Linq;

    public class ProjectProvider : IProjectProvider
    {
        public XDocument LoadProjectDocument(string resolveProjectFilePath)
        {
            return XDocument.Load(resolveProjectFilePath);
        }
    }
}