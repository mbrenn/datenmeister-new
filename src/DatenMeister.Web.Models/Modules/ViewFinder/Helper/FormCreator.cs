using System.Collections;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Models.Forms;

namespace DatenMeister.Web.Models.Modules.ViewFinder.Helper
{
    /// <summary>
    /// Creates a view out of the given extent, elements (collection) or element). 
    /// 
    /// </summary>
    public class FormCreator
    {
        public Form CreateForm(IUriExtent extent)
        {
            return CreateForm(extent.elements());
        }

        public Form CreateForm(IReflectiveSequence elements)
        {
            var result = new Form();
            foreach (var item in elements)
            {
                CreateForm(result, item);
            }

            return result;
        }

        public Form CreateForm(object item)
        {
            var result = new Form();
            CreateForm(result, item);
            return result;
        }

        private void CreateForm(Form result, object item)
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
                            var isAlreadyIn = result.fields.Any(x => x.name == propertyName);
                            if (isAlreadyIn)
                            {
                                continue;
                            }

                            FieldData column = new TextFieldData
                            {
                                name = propertyName,
                                title = propertyName
                            };

                            result.fields.Add(column);
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