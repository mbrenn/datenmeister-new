using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;
using DatenMeister.Web.Models;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreator
    {
        private readonly IUmlNameResolution _nameResolution;
        private readonly IDataLayerLogic _dataLayerLogic;

        public ColumnCreator(IUmlNameResolution nameResolution, IDataLayerLogic _dataLayerLogic)
        {
            _nameResolution = nameResolution;
            this._dataLayerLogic = _dataLayerLogic;
        }

        public ColumnCreationResult FindColumnsForTable(IUriExtent extent)
        {
            return FindColumnsForTable(extent.elements());
        }

        public ColumnCreationResult FindColumnsForTable(IReflectiveSequence elements)
        {
            var result = new ColumnCreationResult();
            foreach (var item in elements)
            {
                EvaluateColumnsForItem(result, item);
            }

            return result;
        }

        public ColumnCreationResult FindColumnsForItem(object item)
        {
            var result = new ColumnCreationResult();
            EvaluateColumnsForItem(result, item);
            return result;
        }

        private void EvaluateColumnsForItem(ColumnCreationResult result, object item)
        {
            // First phase: Get the properties by using the metaclass
            var asElement = item as IElement;
            var metaClass = asElement?.metaclass;

            if (metaClass != null)
            {
                var dataLayer = _dataLayerLogic?.GetDataLayerOfObject(metaClass);
                var metaLayer = _dataLayerLogic?.GetMetaLayerFor(dataLayer);
                var uml = _dataLayerLogic?.Get<_UML>(metaLayer);

                if (uml != null)
                {
                    var properties = metaClass.get(uml.Classification.Classifier.attribute) as IEnumerable;
                    if (properties != null)
                    {
                        foreach (var property in properties.Cast<IObject>())
                        {
                            DataTableColumn column;
                            if (!result.ColumnsOnProperty.TryGetValue(property, out column))
                            {
                                column = new DataTableColumn
                                {
                                    name = property.ToString(),
                                    title = property.get(uml.CommonStructure.NamedElement.name).ToString()
                                };

                                result.ColumnsOnProperty[property] = column;
                            }
                        }
                    }
                }
            }

            // Second phase: Get properties by the object iself
            // This item does not have a metaclass and also no properties, so we try to find them by using the item
            var itemAsAllProperties = item as IObjectAllProperties;
            if (itemAsAllProperties != null)
            {
                var properties = itemAsAllProperties.getPropertiesBeingSet();

                foreach (var property in properties)
                {
                    DataTableColumn column;
                    if (!result.ColumnsOnProperty.TryGetValue(property, out column))
                    {
                        column = new DataTableColumn
                        {
                            name = property.ToString(),
                            title =
                                _nameResolution == null ? property.ToString() : _nameResolution.GetName(property)
                        };

                        result.ColumnsOnProperty[property] = column;
                    }

                    var value = ((IObject) item).get(property);
                    column.isEnumeration |= value is IEnumerable && !(value is string);
                }
            }
        }
    }
}