namespace TestStack.ConventionTests
{
    using System.Xml.Linq;

    public interface IProjectProvider
    {
        XDocument LoadProjectDocument(string resolveProjectFilePath);
    }
}