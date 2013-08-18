namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Linq;
    using System.Reflection;
    using TestStack.ConventionTests.Internal;

    public class DefaultFormatter
    {
        readonly PropertyInfo[] properties;

        public DefaultFormatter(Type type)
        {
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

        public string[] DesribeItem(object result, IConventionFormatContext context)
        {
            return properties.Select(p => context.FormatData(p.GetValue(result, null)).ToString()).ToArray();
        }
    }
}