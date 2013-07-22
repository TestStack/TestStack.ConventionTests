namespace ConventionTests
{
    using System;
    using System.Reflection;

    public class ProjectConventionData
    {
        readonly IProjectLocator projectLocator;

        public ProjectConventionData(IProjectLocator projectLocator)
        {
            this.projectLocator = projectLocator;
        }

        public ProjectConventionData(Assembly assembly) : this(new AssemblyProjectLocator(assembly))
        {

        }

        public ProjectConventionData(Type type) : this(type.Assembly)
        {
        }
    }
}