using System;
using System.Collections;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder.Helper
{
    /// <summary>
    /// Creates a view out of the given extent, elements (collection) or element). 
    /// 
    /// </summary>
    public class FormCreator
    {
        /// <summary>
        /// Stores the creation mode
        /// </summary>
        [Flags]
        public enum CreationMode
        {
            ByMetaClass = 1,
            ByProperties = 2,
            OnlyPropertiesIfNoMetaClass = 4,
            AddMetaClass = 8,
            All = ByMetaClass | ByProperties | AddMetaClass
        }

        public Form CreateForm(IUriExtent extent, CreationMode creationMode)
        {
            return CreateForm(extent.elements(), creationMode);
        }

        public Form CreateForm(IReflectiveCollection elements, CreationMode creationMode)
        {
            if (elements == null) throw new ArgumentNullException(nameof(elements));

            var result = new Form {name = "Items"};
            foreach (var item in elements)
            {
                CreateForm(result, item, creationMode);
            }

            for (var n = 0; n < result.fields.Count; n++)
            {
                var field = result.fields[n];
                if (field is MetaClassElementFieldData)
                {
                    result.fields.RemoveAt(n);
                    result.fields.Add(field);
                }
            }

            return result;
        }

        public Form CreateForm(object item, CreationMode creationMode)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var result = new Form {name = "Item"};
            CreateForm(result, item, creationMode);
            return result;
        }

        /// <summary>
        /// Creates the form out of the given element. 
        /// </summary>
        /// <param name="form">Form which will be extended by the given object</param>
        /// <param name="item">Item being used</param>
        /// <param name="creationMode">Creation mode for the form. Whether by metaclass or ByProperties</param>
        private void CreateForm(Form form, object item, CreationMode creationMode)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // First phase: Get the properties by using the metaclass
            var asElement = item as IElement;
            var metaClass = asElement?.metaclass;
            var wasInMetaClass = false;

            if (creationMode.HasFlag(CreationMode.ByMetaClass)
                && metaClass != null)
            {
                var classifierMethods = ClassifierMethods.GetPropertiesOfClassifier(metaClass).Where(x=> x.isSet("name")).ToList();
                foreach (var property in classifierMethods.OrderBy(x=>x.get("name").ToString()))
                {
                    wasInMetaClass = true;
                    var propertyName = property.get("name").ToString();
                    var isAlreadyIn = form.fields.Any(x => x.name == propertyName);
                    if (isAlreadyIn)
                    {
                        continue;
                    }

                    var column = GetFieldForProperty(property);

                    form.fields.Add(column);
                }
            }

            // Second phase: Get properties by the object iself
            // This item does not have a metaclass and also no properties, so we try to find them by using the item
            var itemAsAllProperties = item as IObjectAllProperties;

            var isByProperties =
                creationMode.HasFlag(CreationMode.ByProperties);
            var isOnlyPropertiesIfNoMetaClass =
                creationMode.HasFlag(CreationMode.OnlyPropertiesIfNoMetaClass);

            if ((isByProperties
                 || (isOnlyPropertiesIfNoMetaClass && !wasInMetaClass))
                && itemAsAllProperties != null)
            {
                var properties = itemAsAllProperties.getPropertiesBeingSet();

                foreach (var property in properties)
                {
                    var column = form.fields.FirstOrDefault(x => x.name == property);
                    if (column == null)
                    {
                        column = new TextFieldData
                        {
                            name = property,
                            title = property
                        };

                        form.fields.Add(column);
                    }

                    var value = ((IObject) item).get(property);
                    column.isEnumeration |= value is IEnumerable && !(value is string);
                }

            }

            // Third phase: Add metaclass
            var isMetaClass = creationMode.HasFlag(CreationMode.AddMetaClass);
            if (isMetaClass && !form.fields.Any(x => x is MetaClassElementFieldData))
            {
                form.fields.Add(new MetaClassElementFieldData());
            }
        }

        /// <summary>
        /// Gets the field data, depending upon the given property
        /// </summary>
        /// <param name="property">Property which is requesting a field</param>
        /// <returns>The field data</returns>
        public FieldData GetFieldForProperty(IElement property)
        {
            var propertyName = property.get("name").ToString();

            var column = new TextFieldData
            {
                name = propertyName,
                title = propertyName
            };

            return column;
        }
    }
}