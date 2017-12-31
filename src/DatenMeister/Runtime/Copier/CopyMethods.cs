using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Runtime.Copier
{
    /// <summary>
    /// Support class supporting the copying of elements
    /// </summary>
    public class CopyMethods
    {
        /// <summary>
        /// Copies the targetElement to the target workspaces
        /// </summary>
        /// <param name="element">Element to be copied</param>
        /// <param name="collection">Collection as target collection</param>
        public static void CopyToTargetWorkspace(IElement element, IReflectiveCollection collection)
        {
            var objectCopier = new ObjectCopier(new MofFactory(collection));
            var copiedElement = objectCopier.Copy(element);
            collection.add(copiedElement);
        }

        /// <summary>
        /// Copies a collection to an elements property, 
        /// if the property already has a collection, then the values are directly added, 
        /// otherwise, the values are stored in an intermediate location
        /// </summary>
        /// <param name="collection">Collection to be parsed</param>
        /// <param name="targetElement">Element to which the elements will be added </param>
        /// <param name="propertyName">Name of the property to which the elements will be added</param>
        public static void CopyToElementsProperty(
            IReflectiveCollection collection, 
            IObject targetElement,
            string propertyName)
        {
            var copier = new ObjectCopier(new MofFactory(targetElement));
            var copiedElements = collection.OfType<IElement>().Select(x => copier.Copy(x));

            if (!targetElement.isSet(propertyName))
            {
                targetElement.set(propertyName, copiedElements);
            }
            else
            {
                var setProperty = targetElement.get(propertyName);
                if (setProperty is IReflectiveCollection targetCollection)
                {
                    foreach (var copiedElement in copiedElements)
                    {
                        targetCollection.add(copiedElement);
                    }
                }
                else
                {
                    var newList = new List<object> {setProperty};
                    newList.AddRange(copiedElements);

                    targetElement.set(propertyName, newList);
                }
            }
        }

        /// <summary>
        /// Copies the targetElement from the source extent to the target workspace
        /// </summary>
        /// <param name="sourceExtent">Source extent to be used</param>
        /// <param name="sourcePath">Path to be looked up. Can also be a container</param>
        /// <param name="targetExtent">Target Extent in which the items shall be stored</param>
        /// <param name="targetPath"></param>
        /// <param name="targetProperty"></param>
        public static void CopyToTargetWorkspace(
            IUriExtent sourceExtent,
            string sourcePath,
            IUriExtent targetExtent,
            string targetPath,
            string targetProperty)
        {
            CopyToTargetWorkspace(
                sourceExtent.elements(), 
                sourcePath, 
                targetExtent.elements(), 
                targetPath,
                targetProperty);
        }

        /// <summary>
        /// Copies the targetElement from the source extent to the target workspace
        /// </summary>
        /// <param name="sourceExtent">Source extent to be used</param>
        /// <param name="sourcePath">Path to be looked up. Can also be a container</param>
        /// <param name="targetExtent">Target Extent in which the items shall be stored</param>
        /// <param name="targetPath"></param>
        /// <param name="targetProperty"></param>
        public static void CopyToTargetWorkspace(
            IReflectiveCollection sourceExtent,
            string sourcePath,
            IReflectiveCollection targetExtent,
            string targetPath,
            string targetProperty)
        {
            var sourceElement = NamedElementMethods.GetByFullName(sourceExtent, sourcePath);
            var copiedElement = new ObjectCopier(new MofFactory(targetExtent)).Copy(sourceElement);
            var targetElement = NamedElementMethods.GetByFullName(targetExtent, targetPath);
            if (!targetElement.isSet(targetProperty))
            {
                var newList = new object[] {copiedElement};
                targetElement.set(targetProperty, newList);
            }
            else
            {
                var setProperty = targetElement.get(targetProperty);
                if (setProperty is IReflectiveCollection collection)
                {
                    collection.add(copiedElement);
                }
                else
                {
                    var newList = new object[] {setProperty, copiedElement};
                    targetElement.set(targetProperty, newList);
                }
            }
        }
    }
}