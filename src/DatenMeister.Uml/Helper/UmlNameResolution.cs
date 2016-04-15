using System;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public class UmlNameResolution : IUmlNameResolution
    {
        private IDataLayerLogic _dataLayerLogic;

        public UmlNameResolution(IDataLayerLogic dataLayerLogic)
        {
            _dataLayerLogic = dataLayerLogic;
        }

        public string GetName(IObject element)
        {
            // Returns the name by the uml logic. 
            var dataLayer = _dataLayerLogic?.GetDataLayerOfObject(element);
            var metaLayer = _dataLayerLogic?.GetMetaLayerFor(dataLayer);
            var uml = _dataLayerLogic?.Get<_UML>(metaLayer);
            if (uml != null && element.isSet(uml.CommonStructure.NamedElement.name))
            {
                var result = element.get(uml.CommonStructure.NamedElement.name);
                if (result != null)
                {
                    return result.ToString();
                }
            }

            // If the element is not uml induced or the property is empty, check by
            // the default "name" property
            if (element.isSet("name"))
            {
                return element.get("name").ToString();
            }

            // Ok, finally, we don't know what to do, so request retrieve the name just via ToString
            return element.ToString();
        }

        public string GetName(object element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var asObject = element as IObject;
            return asObject == null ? element.ToString() : GetName(asObject);
        }
    }
}