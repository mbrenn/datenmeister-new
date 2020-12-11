using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Uml.Helper;

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
                DefaultClassifierHints.RemoveFromExtentOrElement(container ?? (IObject) extent!, value);

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
            var options = new CopyOption {CloneAllReferences = false};
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
        /// <param name="containerPropertyName">The property to which the element will be added when
        /// added to a container</param>
        /// <returns>The created element</returns>
        public static IElement AddNewItemAsReferenceToInstanceProperty(
            IElement item, 
            string propertyName, 
            IElement? typeForCreation, 
            IObject container)
        {
            var factory = new MofFactory(item);
            var newElement = factory.create(typeForCreation);

            // Check, if the element is a compositing property
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

            // Checks, if there is a composition
            if (isComposite)
            {
                item.set(propertyName, newElement);
            }
            else
            {
                DefaultClassifierHints.AddToExtentOrElement(container, newElement);
                item.set(propertyName, newElement);
            }

            return newElement;
        }
    }
}