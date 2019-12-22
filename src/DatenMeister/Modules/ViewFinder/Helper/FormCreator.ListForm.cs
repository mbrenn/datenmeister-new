#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder.Helper
{
    public partial class FormCreator
    {
        /// <summary>
        /// Creates a list form for a certain metaclass
        /// </summary>
        /// <param name="metaClass"></param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForMetaClass(IElement metaClass, CreationMode creationMode)
        {
            if (!creationMode.HasFlag(CreationMode.ByMetaClass))
            {
                throw new InvalidOperationException("The list form will only be created for the metaclass");
            }

            var result = _factory.create(_formAndFields.__ListForm);

            var nameOfListForm = NamedElementMethods.GetName(metaClass);
            result.set(_FormAndFields._ListForm.title, nameOfListForm);
            result.set(_FormAndFields._ListForm.name, nameOfListForm);
            AddToFormByMetaclass(result, metaClass, creationMode);

            var defaultType = _factory.create(_formAndFields.__DefaultTypeForNewElement);
            defaultType.set(_FormAndFields._DefaultTypeForNewElement.metaClass, metaClass);
            defaultType.set(_FormAndFields._DefaultTypeForNewElement.name, NamedElementMethods.GetName(metaClass));
            result.set(_FormAndFields._ListForm.defaultTypesForNewElements, new[] {defaultType});

            return result;
        }

        /// <summary>
        /// Creates the list form out of the elements in the reflective collection.
        /// Supports the creation by the metaclass and by the object's properties
        /// </summary>
        /// <param name="elements">Elements to be queried</param>
        /// <param name="creationMode">The used creation mode</param>
        /// <returns>The created list form </returns>
        public IElement CreateListFormForElements(IReflectiveCollection elements, CreationMode creationMode)
        {
            var cache = new FormCreatorCache();
            var alreadyVisitedMetaClasses = new HashSet<IElement>();

            IObject? firstElementMetaClass = null;
            var metaClassAdded = false;
            var result = _factory.create(_formAndFields.__ListForm);

            foreach (var element in elements.OfType<IObject>())
            {
                var metaClass = (element as IElement)?.getMetaClass();
                if (firstElementMetaClass == null || !creationMode.HasFlag(CreationMode.AddMetaClass))
                {
                    // If this is the first element or when the creator does not allow the addition
                    // of a metaclass
                    firstElementMetaClass = metaClass;
                }
                else if (firstElementMetaClass != metaClass && !metaClassAdded)
                {
                    metaClassAdded = true;
                    // Create the metaclass as a field
                    var metaClassField = _factory.create(_formAndFields.__MetaClassElementFieldData);
                    result.get<IReflectiveSequence>(_FormAndFields._Form.field).add(0, metaClassField);
                }

                if (creationMode.HasFlag(CreationMode.ByMetaClass) && metaClass != null)
                {
                    if (alreadyVisitedMetaClasses.Contains(metaClass))
                    {
                        continue;
                    }

                    alreadyVisitedMetaClasses.Add(metaClass);
                    AddToFormByMetaclass(result, metaClass, creationMode);
                }
                else if (creationMode.HasFlag(CreationMode.ByProperties))
                {
                    AddToForm(result, element,
                        creationMode & ~CreationMode.ByMetaClass, cache);
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a list form for a certain metaclass being used inside an extent form
        /// </summary>
        /// <param name="metaClass"></param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForPropertyInObject(IElement metaClass, string propertyName, CreationMode creationMode)
        {
            if (!creationMode.HasFlag(CreationMode.ByMetaClass))
            {
                throw new InvalidOperationException("The list form will only be created for the metaclass");
            }

            var result = _factory.create(_formAndFields.__ListForm);
            AddToFormByMetaclass(result, metaClass, creationMode);
            result.set(_FormAndFields._ListForm.property, propertyName);
            result.set(_FormAndFields._ListForm.title, $"{propertyName} - {NamedElementMethods.GetName(metaClass)}");
            return result;
        }

        /// <summary>
        /// Creates a list form for a certain metaclass being used inside an extent form
        /// </summary>
        /// <param name="property">Property to be evaluated</param>
        /// <param name="creationMode"></param>
        public IElement CreateListFormForProperty(IElement property, CreationMode creationMode)
        {
            if (!creationMode.HasFlag(CreationMode.ByMetaClass))
            {
                throw new InvalidOperationException("The list form will only be created for the metaclass");
            }

            var propertyName = property.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
            var propertyType = PropertyMethods.GetPropertyType(property);

            var result = _factory.create(_formAndFields.__ListForm);
            AddToFormByMetaclass(result, propertyType, creationMode);
            result.set(_FormAndFields._ListForm.property, propertyName);
            result.set(_FormAndFields._ListForm.title, $"{propertyName}");
            return result;
        }
        
    }
}