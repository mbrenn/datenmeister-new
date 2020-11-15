using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.FastViewFilter
{
    public class FastViewFilterTranslator
    {
        public string TranslateType(IObject metaClassType)
        {
            if (metaClassType.Equals(_DatenMeister.TheOne.FastViewFilters.__PropertyContainsFilter))
            {
                return "Property contains...";
            }

            if (metaClassType.Equals(_DatenMeister.TheOne.FastViewFilters.__PropertyComparisonFilter))
            {
                return "Property value compares...";
            }

            var fullName = NamedElementMethods.GetFullName(metaClassType);
            return fullName;
        }

        /// <summary>
        /// Translates the filter to a filter text
        /// </summary>
        /// <param name="fastFilter">Fastfilter t</param>
        /// <returns></returns>
        public string TranslateFilter(IElement fastFilter)
        {
            var metaClass = fastFilter.getMetaClass();
            if (metaClass == null) return "Unknown";
            
            if (metaClass.Equals(_DatenMeister.TheOne.FastViewFilters.__PropertyComparisonFilter))
            {
                var property = fastFilter.get<string>(_DatenMeister._FastViewFilters._PropertyComparisonFilter.Property);
                var contains = fastFilter.get<string>(_DatenMeister._FastViewFilters._PropertyComparisonFilter.Value);
                var comparisonType = fastFilter.get<string>(_DatenMeister._FastViewFilters._PropertyComparisonFilter.ComparisonType);
                return $"'{property}' {comparisonType.ToLower()} '{contains}'";
            }

            if (metaClass.Equals(_DatenMeister.TheOne.FastViewFilters.__PropertyContainsFilter))
            {
                var property = fastFilter.get<string>(_DatenMeister._FastViewFilters._PropertyContainsFilter.Property);
                var contains = fastFilter.get<string>(_DatenMeister._FastViewFilters._PropertyContainsFilter.Value);
                return $"'{property}' contains '{contains}'";
            }

            return fastFilter.ToString();
        }
    }
}