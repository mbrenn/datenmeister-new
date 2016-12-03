using System;
using System.Collections;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
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

        [Flags]
        public enum CreationMode
        {
            ByMetaClass = 1,
            ByProperties = 2,
            OnlyPropertiesIfNoMetaClass = 5,
            All = ByMetaClass | ByProperties
        }

        public Form CreateForm(IUriExtent extent, CreationMode creationMode)
        {
            return CreateForm(extent.elements(), creationMode);
        }

        public Form CreateForm(IReflectiveSequence elements, CreationMode creationMode)
        {
            var result = new Form();
            foreach (var item in elements)
            {
                CreateForm(result, item, creationMode);
            }

            return result;
        }

        public Form CreateForm(object item, CreationMode creationMode)
        {
            var result = new Form();
            CreateForm(result, item, creationMode);
            return result;
        }

        private void CreateForm(Form result, object item, CreationMode creationMode)
        {
            // First phase: Get the properties by using the metaclass
            var asElement = item as IElement;
            var metaClass = asElement?.metaclass;
            var wasInMetaClass = false;

            if (creationMode.HasFlag(CreationMode.ByMetaClass)
                && metaClass != null)
            {
                var classifierMethods = ClassifierMethods.GetPropertiesOfClassifier(metaClass).Where(x=> x.isSet("name")).ToList();
                foreach (var property in classifierMethods
                    .OrderBy(x=>x.get("name").ToString()))
                {
                    wasInMetaClass = true;
                    var propertyName = property.get("name").ToString();
                    var isAlreadyIn = result.fields.Any(x => x.name == propertyName);
                    if (isAlreadyIn)
                    {
                        continue;
                    }

                    var column = new TextFieldData
                    {
                        name = propertyName,
                        title = propertyName
                    };

                    result.fields.Add(column);
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
                    var column = result.fields.FirstOrDefault(x => x.name == property);
                    if (column == null)
                    {
                        column = new TextFieldData
                        {
                            name = property,
                            title = property
                        };

                        result.fields.Add(column);
                    }

                    var value = ((IObject) item).get(property);
                    column.isEnumeration |= value is IEnumerable && !(value is string);
                }
            }
        }
    }
}