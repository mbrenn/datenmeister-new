using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.FastViewFilter
{
    public class FastViewFilterTranslator
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        public FastViewFilterTranslator(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        public string TranslateType(IObject metaClassType)
        {
            if (metaClassType.Equals(_FastViewFilters.TheOne.__PropertyContainsFilter))
            {
                return "Property contains...";
            }

            if (metaClassType.Equals(_FastViewFilters.TheOne.__PropertyComparisonFilter))
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
            
            if (metaClass.Equals(_FastViewFilters.TheOne.__PropertyComparisonFilter))
            {
                var property = fastFilter.get<string>(_FastViewFilters._PropertyComparisonFilter.Property);
                var contains = fastFilter.get<string>(_FastViewFilters._PropertyComparisonFilter.Value);
                var comparisonType = fastFilter.get<string>(_FastViewFilters._PropertyComparisonFilter.ComparisonType);
                return $"'{property}' {comparisonType.ToLower()} '{contains}'";
            }

            if (metaClass.Equals(_FastViewFilters.TheOne.__PropertyContainsFilter))
            {
                var property = fastFilter.get<string>(_FastViewFilters._PropertyContainsFilter.Property);
                var contains = fastFilter.get<string>(_FastViewFilters._PropertyContainsFilter.Value);
                return $"'{property}' contains '{contains}'";
            }

            return fastFilter.ToString();
        }
    }
}