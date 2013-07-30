namespace TestStack.ConventionTests.Helpers
{
    using System.Reflection;

    public interface IProjectLocator
    {
        string ResolveProjectFilePath(Assembly assembly);
    }
}