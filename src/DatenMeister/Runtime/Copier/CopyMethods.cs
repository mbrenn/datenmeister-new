using System;
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
        /// <param name="sourceElement">Element to be copied</param>
        /// <param name="targetCollection">Collection as target collection</param>
        public static void CopyToTargetWorkspace(IElement sourceElement, IReflectiveCollection targetCollection)
        {
            var objectCopier = new ObjectCopier(new MofFactory(targetCollection));
            var copiedElement = objectCopier.Copy(sourceElement);
            targetCollection.add(copiedElement);
        }

        /// <summary>
        /// Copies a collection to an elements property of the targetElement,
        /// If the property is not set, the property will be directly set by the copy
        /// If the property is set, then the copied elements will be added to the already existing values
        /// </summary>
        /// <param name="collection">Collection to be parsed</param>
        /// <param name="targetElement">Element to which the elements will be added </param>
        /// <param name="targetPropertyName">Name of the property to which the elements will be added</param>
        /// <param name="copyOptions">Copies the options</param>
        public static void CopyToElementsProperty(
            IReflectiveCollection collection,
            IObject targetElement,
            string targetPropertyName,
            CopyOption copyOptions)
        {
            var copier = new ObjectCopier(new MofFactory(targetElement));
            var copiedElements = collection.OfType<IElement>().Select(x => copier.Copy(x, copyOptions));

            if (!targetElement.isSet(targetPropertyName))
            {
                targetElement.set(targetPropertyName, copiedElements);
            }
            else
            {
                var setProperty = targetElement.get(targetPropertyName);
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

                    targetElement.set(targetPropertyName, newList);
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
        /// <param name="sourcePath">Path to be looked up to retrieve the source element</param>
        /// <param name="targetExtent">Target Reflective Collection in which the items shall be stored</param>
        /// <param name="targetPath">Path of the element in which the elements shall be stored</param>
        /// <param name="targetProperty">Target Property in which the elements will be stored</param>
        public static void CopyToTargetWorkspace(
            IReflectiveCollection sourceExtent,
            string sourcePath,
            IReflectiveCollection targetExtent,
            string targetPath,
            string targetProperty)
        {
            var sourceElement = NamedElementMethods.GetByFullName(sourceExtent, sourcePath)
                                ?? throw new InvalidOperationException("sourcePath does not show to source element");
            var copiedElement = new ObjectCopier(new MofFactory(targetExtent)).Copy(sourceElement);
            var targetElement = NamedElementMethods.GetByFullName(targetExtent, targetPath)
                                ?? throw new InvalidOperationException("targetPath does not show to source element");

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
                    var newList = new[] {setProperty, copiedElement};
                    targetElement.set(targetProperty, newList);
                }
            }
        }
    }
}