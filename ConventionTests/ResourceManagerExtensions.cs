namespace ConventionTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Resources;

    public static class ResourceManagerExtensions
    {
        public static IEnumerable<MissingResource> GetMissingLocalisationResources(this ResourceManager resourceManager,
                                                                                   string languageName, params string[] languagesToCheck)
        {
            var masterLanguage = resourceManager.GetResourceSet(new CultureInfo(languageName), true, true);
            var languageSets = languagesToCheck.Select(l => new
                {
                    ResourceSet = resourceManager.GetResourceSet(new CultureInfo(l), true, true),
                    Language = l
                });

            return from resource in masterLanguage.Cast<DictionaryEntry>()
                   from languageSet in languageSets
                   where string.IsNullOrEmpty(languageSet.ResourceSet.GetString((string)resource.Key))
                   select new MissingResource
                       {
                           Language = languageSet.Language,
                           ResourceKey = resource.Key.ToString()
                       };
        }
    }
}