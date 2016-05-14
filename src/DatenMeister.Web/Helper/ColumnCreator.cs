using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.Web.Models;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreator
    {
        private readonly IUmlNameResolution _nameResolution;
        private readonly IDataLayerLogic _dataLayerLogic;
        private readonly IWorkspaceCollection _workspaceCollection;

        public ColumnCreator(IUmlNameResolution nameResolution, IDataLayerLogic dataLayerLogic, IWorkspaceCollection workspaceCollection)
        {
            _nameResolution = nameResolution;
            _dataLayerLogic = dataLayerLogic;
            _workspaceCollection = workspaceCollection;
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

        public static string ConvertPropertyToColumnName(object property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            var asElement = property as IElement;
            if (asElement != null)
            {
                // Property is an IElement, so retrieve the information by url
                return string.Format($"#uml#{asElement.GetUri()}");
            }
            else
            {
                // Property is a string, so return it by ToString()
                var propertyAsString = property.ToString();

                if (propertyAsString.StartsWith("#"))
                {
                    throw new InvalidOperationException("Property may not start with a '#': " + propertyAsString);
                }

                return propertyAsString;
            }
        }

        public object ConvertColumnNameToProperty(string property)
        {
            if (property.StartsWith("#"))
            {
                if (property.StartsWith("#uml#"))
                {
                    property = property.Substring("#uml#".Length);
                    return _workspaceCollection.FindItem(property);
                }

                throw new InvalidOperationException($"Property with name '{property}' starts with '#' but format is not known. ");
            }

            return property;
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

                if (uml != null && metaClass.isSet(uml.Classification.Classifier.attribute))
                {
                    var properties = metaClass.get(uml.Classification.Classifier.attribute) as IEnumerable;
                    if (properties != null)
                    {
                        foreach (var property in properties.Cast<IObject>())
                        {
                            DataField column;
                            if (!result.ColumnsOnProperty.TryGetValue(property, out column))
                            {
                                column = new DataField
                                {
                                    name = ConvertPropertyToColumnName(property),
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
                    DataField column;
                    if (!result.ColumnsOnProperty.TryGetValue(property, out column))
                    {
                        column = new DataField
                        {
                            name = ConvertPropertyToColumnName(property),
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