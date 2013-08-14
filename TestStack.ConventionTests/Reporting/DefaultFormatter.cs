namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class DefaultFormatter
    {
        readonly PropertyInfo[] properties;
        readonly Type type;

        public DefaultFormatter(Type type)
        {
            this.type = type;
            properties = type.GetProperties();
        }

        // TODO: this is a very crappy name for a method
        public string[] DesribeType()
        {
            return properties.Select(Describe).ToArray();
        }

        string Describe(PropertyInfo property)
        {
            return property.Name.Replace('_', ' ');
        }

        public string[] DesribeItem(object result)
        {
            return properties.Select(p => p.GetValue(result, null).ToString()).ToArray();
        }
    }
}