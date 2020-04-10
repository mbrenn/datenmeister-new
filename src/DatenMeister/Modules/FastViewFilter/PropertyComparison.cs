using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.FastViewFilter;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.FastViewFilter
{
    public class PropertyComparison : IFastFilter
    {
        private readonly IObject _filterObject;

        public PropertyComparison(IObject filterObject)
        {
            _filterObject = filterObject;
        }

        public bool IsFiltered(IObject value)
        {
            var filterObject = DotNetConverter.ConvertToDotNetObject<PropertyComparisonFilter>(_filterObject);
            if (filterObject.Property == null)
            {
                // To avoid exceptions
                return true;
            }

            var propertyValue = value.getOrDefault<string>(filterObject.Property);

            switch (filterObject.ComparisonType)
            {
                case ComparisonType.Equal:
                    return propertyValue == filterObject.Value;
                case ComparisonType.GreaterThan:
                    return string.CompareOrdinal(propertyValue, filterObject.Value) > 0;
                case ComparisonType.LighterThan:
                    return string.CompareOrdinal(propertyValue, filterObject.Value) < 0;
                case ComparisonType.GreaterOrEqualThan:
                    return string.CompareOrdinal(propertyValue, filterObject.Value) >= 0;
                case ComparisonType.LighterOrEqualThan:
                    return string.CompareOrdinal(propertyValue, filterObject.Value) <= 0;
                default:
                    return true;
            }
        }
    }
}