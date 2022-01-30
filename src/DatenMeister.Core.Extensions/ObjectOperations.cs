using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Modules.DefaultTypes
{
    public static class ObjectOperations
    {
        /// <summary>
        /// Moves the object to the target 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetContainer">Defines the target container receiving the object</param>
        public static void MoveObject(IObject value, IObject targetContainer)
        {
            var extent = value.GetExtentOf();
            var container = (value as IElement)?.container();

            if (container != null || extent != null)
            {
                DefaultClassifierHints.RemoveFromExtentOrElement(container ?? (IObject)extent!, value);

                if (value is MofObject mofObject)
                {
                    mofObject.Extent = null;
                }
            }

            DefaultClassifierHints.AddToExtentOrElement(targetContainer, value);
        }

        /// <summary>
        /// Copies the object to the target 
        /// </summary>
        /// <param name="value">The value to be copied</param>
        /// <param name="targetContainer">Defines the target container receiving the object</param>
        public static void CopyObject(IObject value, IObject targetContainer)
        {
            var options = new CopyOption { CloneAllReferences = false };
            var copied = ObjectCopier.Copy(new MofFactory(targetContainer), value, options);
            DefaultClassifierHints.AddToExtentOrElement(targetContainer, copied);
        }

        /// <summary>
        /// Creates a new instance of an element having the Type subType.
        /// The element will be added to the property. If the property is not a compositing element
        /// then the element will be added to the containing element otherwise it will be directly added. 
        /// </summary>
        /// <param name="item">Item which will receive the new element</param>
        /// <param name="propertyName">Property to which the new element will be added</param>
        /// <param name="typeForCreation">Type of the element that will be instantiiated</param>
        /// <param name="container">Container element to which the element will be added if the property is
        /// not a composition</param>
        /// <param name="asCollection">Flag whether the element shall added as a entry within a collection
        /// or whether the element will be the only element</param>
        /// <returns>The created element</returns>
        public static IElement AddNewItemAsReferenceToInstanceProperty(
            IElement item,
            string propertyName,
            IElement? typeForCreation,
            IObject container,
            bool asCollection)
        {
            var factory = new MofFactory(item);
            var newElement = factory.create(typeForCreation);

            return AddItemReferenceToInstanceProperty(item, propertyName, container, newElement, asCollection);
        }

        /// <summary>
        /// Adds an existing item as reference or as composite to the 
        /// </summary>
        /// <param name="item">The item to which the new element shall be added</param>
        /// <param name="propertyName">The property under which the new element will be added</param>
        /// <param name="newElement">The element to be added</param>
        /// <param name="asCollection">true, if the newElement shall be added as a collective item</param>
        /// <returns>The added element itself</returns>
        public static IElement AddItemReferenceToInstanceProperty(
            IElement item,
            string propertyName,
            IElement newElement,
            bool asCollection)
        {
            return AddItemReferenceToInstanceProperty(item, propertyName, null, newElement, asCollection);
        }

        private static IElement AddItemReferenceToInstanceProperty(
            IElement item,
            string propertyName,
            IObject? container,
            IElement newElement,
            bool asCollection)
        {
            // Check, if the element is a compositing property
            var isComposite = IsCompositeProperty(item, propertyName);

            // Checks, if there is a composition
            if (isComposite)
            {
                if (asCollection)
                {
                    item.AddCollectionItem(propertyName, newElement);
                }
                else
                {
                    item.set(propertyName, newElement);
                }
            }
            else
            {
                container ??= item.container()
                              ?? (IObject?)item.GetExtentOf()
                              ?? throw new InvalidOperationException("Container is not given");

                DefaultClassifierHints.AddToExtentOrElement(container, newElement);

                if (asCollection)
                {
                    item.AddCollectionItem(propertyName, newElement);
                }
                else
                {
                    item.set(propertyName, newElement);
                }
            }

            return newElement;
        }

        /// <summary>
        /// Checks whether the item has a composite property
        /// </summary>
        /// <param name="item">Item to be evaluated</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if the item has a composite property</returns>
        public static bool IsCompositeProperty(IElement item, string propertyName)
        {
            bool isComposite;
            var metaClass = item.getMetaClass();
            if (metaClass == null)
            {
                isComposite = true;
            }
            else
            {
                var property = ClassifierMethods.GetPropertyOfClassifier(metaClass, propertyName);
                isComposite = property == null || PropertyMethods.IsComposite(property);
            }

            return isComposite;
        }
    }
}