namespace TestStack.ConventionTests.Internal
{
    using System.Reflection;

    public interface IProjectLocator
    {
        string ResolveProjectFilePath(Assembly assembly);
    }
}