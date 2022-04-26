using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace IUGOCare.Application.Common.Constants
{
    public static class EmailTemplateConfiguration
    {
        public const string EmailTemplate = "SendGrid:EmailTemplate";

        public static string GetResourceString(string key, string language)
        {
            CultureInfo ci = new CultureInfo(string.IsNullOrWhiteSpace(language) ? "en" : language);
            ResourceManager resourceManager = new ResourceManager("IUGOCare.Application.Common.Resources.Resource",
                                                    Assembly.GetExecutingAssembly());
            return resourceManager.GetString(key, ci);
        }

        public static List<string> GetResourceStrings(string key, string language)
        {
            CultureInfo ci = new CultureInfo(string.IsNullOrWhiteSpace(language) ? "en" : language);
            ResourceManager resourceManager = new ResourceManager("IUGOCare.Application.Common.Resources.Resource",
                                                    Assembly.GetExecutingAssembly());
            var list = new List<DictionaryEntry>();
            var set = resourceManager.GetResourceSet(ci, true, true);
            foreach (var item in set)
            {
                var de = (DictionaryEntry)item;
                if (de.Key.ToString().StartsWith(key))
                    list.Add(de);
            }
            return list
                    .OrderBy(de => de.Key.ToString())
                    .Select(de => de.Value.ToString())
                    .ToList();
        }
    }
}
