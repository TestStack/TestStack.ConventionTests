namespace TestStack.ConventionTests.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.ConventionData;

    public enum ConfigurationType
    {
        Unknown,
        All,
        Global,
        Release,
        Debug
    }

    public class ConfigurationHasSpecificValue : IConvention<ProjectPropertyGroups>
    {
        public ConfigurationType Type { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }

        public ConfigurationHasSpecificValue(ConfigurationType type, string property, string value)
        {
            Type = type;
            Property = property;
            Value = value;
        }


        public void Execute(ProjectPropertyGroups data, IConventionResultContext result)
        {
            foreach (var group in data.PropertyGroups.Where(Match))
            {
                if (group[Property] == null)
                {
                    continue;
                }

                result.Is(string.Format("{0} property in {1} must have a value of {2}", this.Property, group.Name, this.Value), 
                    group[Property].Equals(this.Value) ? new string[]{} : new[] {string.Format("{0}:{1}", this.Property, group[Property])});   
            }            
        }

        private bool Match(ProjectPropertyGroup group)
        {
            switch (this.Type)
            {
                case ConfigurationType.All:
                    return true;
                case ConfigurationType.Global:
                    return group.Global;
                case ConfigurationType.Release:
                    return group.Release;
                case ConfigurationType.Debug:
                    return group.Debug;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string ConventionReason
        {
            get
            {
                return
                    "Make sure projects have a specific value defined in all release configuration sections";
            }
        }
    }
}