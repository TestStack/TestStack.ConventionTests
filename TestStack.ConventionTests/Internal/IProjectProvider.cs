namespace TestStack.ConventionTests.Internal
{
    using System.Xml.Linq;

    public interface IProjectProvider
    {
        XDocument LoadProjectDocument();
        string GetName();
    }
}