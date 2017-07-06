namespace TestStack.ConventionTests.ConventionData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class ProjectPropertyGroup
    {
        readonly Dictionary<string, string> properties;

        public ProjectPropertyGroup(string condition, IEnumerable<KeyValuePair<string, string>> properties)
        {
            Condition = condition;
            this.properties = new Dictionary<string, string>();
            foreach (var pair in properties)
            {
                this.properties.Add(pair.Key, pair.Value);
            }
        }

        public string Condition { get; set; }

        public bool Debug
        {
            get
            {
                return !Global && Condition.IndexOf("debug", StringComparison.OrdinalIgnoreCase) > -1;
            }
        }

        public bool Global
        {
            get { return this.Condition == null; }
        }

        public string Name
        {
            get
            {
                if (this.Global)
                {
                    return "Global";
                }

                var entries = this.Condition.Split(new[]{'\''}, StringSplitOptions.RemoveEmptyEntries).Where(item => item.Trim().Length > 0);
                return entries.Last();
            }
        }

        public bool Release
        {
            get
            {
                return !Global && Condition.IndexOf("release", StringComparison.OrdinalIgnoreCase) > -1;
            }
        }

        public string this[string property]
        {
            get
            {
                if (!properties.ContainsKey(property))
                {
                    return null;
                }

                return properties[property];
            }
        }
    }
}