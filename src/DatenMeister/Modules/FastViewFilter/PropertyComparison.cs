using DatenMeister.Core.EMOF.Interface.Reflection;

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
            throw new System.NotImplementedException();
        }
    }
}