using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;

namespace DatenMeister.WPF.Modules.UserInteractions
{
    /// <summary>
    /// Defines some helper methods depending on the given element
    /// </summary>
    public abstract class BaseElementInteractionHandler : IElementInteractionsHandler
    {
        /// <summary>
        /// Gets or sets the class name of the elements that are in scópe of the BaseElementInteractionHandler
        /// </summary>
        protected IElement? OnlyElementsOfType { get; set; }

        /// <inheritdoc />
        public abstract IEnumerable<IElementInteraction> GetInteractions(IObject element);

        /// <summary>
        /// Returns whether the given element is relevant for the given interaction handler.
        /// It uses the information from the OnlyElementsOfType instance.
        /// This method can be called by derived classes.
        /// </summary>
        /// <param name="element">Element to be checked</param>
        /// <returns>true, if relevant</returns>
        protected bool IsRelevant(IObject element)
        {
            if (OnlyElementsOfType != null && element is IElement elementAsElement)
            {
                var metaClassElement = elementAsElement.getMetaClass();
                if (metaClassElement == null)
                {
                    return false;
                }
                
                if (!ClassifierMethods.IsSpecializedClassifierOf(elementAsElement.getMetaClass(), OnlyElementsOfType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}