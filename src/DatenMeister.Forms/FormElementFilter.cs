using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Forms;

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
        if (formElement.getOrDefault<bool>(_Forms._TableForm.includeDescendents))
        {
            elements = elements.GetAllDescendantsIncludingThemselves();
        }

        // Extent shall be shown
        elements = FilterByMetaClass(formElement, elements).WhenElementIsObject();
            
        // Now performs the sorting
        var sortingOrder =
            formElement.getOrDefault<IReflectiveCollection>(
                _Forms._TableForm.sortingOrder);
        if (sortingOrder != null)
        {
            var sortingColumnNames =
                sortingOrder
                    .OfType<IElement>()
                    .Select(x =>
                        (x.getOrDefault<bool>(_Forms._SortingOrder.isDescending)
                            ? "!"
                            : "") +
                        x.getOrDefault<string>(_Forms._SortingOrder.name))
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
            formElement.getOrDefault<bool>(_Forms._TableForm.noItemsWithMetaClass);

        // If form  defines constraints upon metaclass, then the filtering will occur here
        var metaClass = formElement.getOrDefault<IElement?>(_Forms._TableForm.metaClass);

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