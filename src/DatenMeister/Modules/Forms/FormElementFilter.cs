using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Modules.Forms
{
    /// <summary>
    /// Performs the filtering of the elements according the form rules
    /// </summary>
    public static class FormElementFilter
    {
        /// <summary>
        /// Performs the filtering of the elements
        /// </summary>
        /// <param name="formElement"></param>
        /// <param name="elements">Elements to be filter</param>
        /// <returns>The reflective collection containing the filtered elements</returns>
        public static IReflectiveCollection FilterElements(IObject formElement, IReflectiveCollection elements)
        {
            if (formElement.getOrDefault<bool>(_DatenMeister._Forms._ListForm.includeDescendents))
            {
                elements = elements.GetAllDescendantsIncludingThemselves();
            }

            // Extent shall be shown
            elements = FilterByMetaClass(formElement, elements).WhenElementIsObject();
            
            // Now performs the sorting
            var sortingOrder =
                formElement.getOrDefault<IReflectiveCollection>(
                    _DatenMeister._Forms._ListForm.sortingOrder);
            if (sortingOrder != null)
            {
                var sortingColumnNames =
                    sortingOrder
                        .OfType<IElement>()
                        .Select(x =>
                            (x.getOrDefault<bool>(_DatenMeister._Forms._SortingOrder.isDescending)
                                ? "!"
                                : "") +
                            x.getOrDefault<string>(_DatenMeister._Forms._SortingOrder.name))
                        .Where(x => !string.IsNullOrEmpty(x) && x != "!");
                elements = elements.OrderElementsBy(sortingColumnNames);
            }
            
            return elements;
        }

        /// <summary>
        /// Gets the collection and return the collection by the filtered metaclasses. If the metaclass
        /// is not defined, then null is returned
        /// </summary>
        /// <param name="formElement"></param>
        /// <param name="collection">Collection to be filtered</param>
        /// <returns>The filtered metaclasses</returns>
        private static IReflectiveCollection FilterByMetaClass(IObject formElement, IReflectiveCollection collection)
        {
            if (formElement == null) throw new InvalidOperationException("EffectiveForm == null");
            
            var noItemsWithMetaClass =
                formElement.getOrDefault<bool>(_DatenMeister._Forms._ListForm.noItemsWithMetaClass);

            // If form  defines constraints upon metaclass, then the filtering will occur here
            var metaClass = formElement.getOrDefault<IElement?>(_DatenMeister._Forms._ListForm.metaClass);

            if (metaClass != null)
            {
                return collection.WhenMetaClassIs(metaClass);
            }

            if (noItemsWithMetaClass)
            {
                return collection.WhenMetaClassIs(null);
            }

            return collection;
        }
    }
}