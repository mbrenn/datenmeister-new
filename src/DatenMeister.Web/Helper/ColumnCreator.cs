using System;
using System.Collections;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreator
    {
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
                if (metaClass.isSet(_UML._Classification._Classifier.attribute))
                {
                    var properties = metaClass.get(_UML._Classification._Classifier.attribute) as IEnumerable;
                    if (properties != null)
                    {
                        foreach (var property in properties.Cast<IObject>())
                        {
                            var propertyName = property.get("name").ToString();
                            FieldData column;
                            if (!result.ColumnsOnProperty.TryGetValue(propertyName, out column))
                            {
                                column = new TextFieldData
                                {
                                    name = propertyName,
                                    title = propertyName
                                };

                                result.ColumnsOnProperty[propertyName] = column;
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
                    FieldData column;
                    if (!result.ColumnsOnProperty.TryGetValue(property, out column))
                    {
                        column = new TextFieldData
                        {
                            name = property,
                            title = property
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