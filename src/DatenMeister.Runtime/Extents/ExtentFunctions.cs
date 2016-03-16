using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;

namespace DatenMeister.Runtime.Extents
{
    public class ExtentFunctions
    {
        private readonly IDataLayerLogic _dataLayerLogic;
        public ExtentFunctions(IDataLayerLogic dataLayerLogic)
        {
            _dataLayerLogic = dataLayerLogic;
        }

        public IList<IElement> GetCreatableTypes(IUriExtent extent)
        {
            var dataLayer = _dataLayerLogic.GetDataLayerOfExtent(extent);
            var typeLayer = _dataLayerLogic.GetMetaLayerFor(dataLayer);
            var umlLayer= _dataLayerLogic.GetMetaLayerFor(typeLayer);

            var uml = _dataLayerLogic.Get<_UML>(umlLayer);
            var classType = uml.StructuredClassifiers.__Class;

            return
                _dataLayerLogic.GetExtentsForDatalayer(typeLayer)
                    .SelectMany(x => x.elements().WhenMetaClassIs(classType))
                    .Cast<IElement>()
                    .ToList();
        }
    }
}