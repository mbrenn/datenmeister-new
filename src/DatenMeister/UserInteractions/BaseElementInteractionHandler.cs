using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;

namespace DatenMeister.UserInteractions
{
    /// <summary>
    /// Defines some helper methods depending on the given element
    /// </summary>
    public abstract class BaseElementInteractionHandler : IElementInteractionsHandler
    {
        /// <summary>
        /// Gets or sets the class name of the elements that are in scópe of the BaseElementInteractionHandler
        /// </summary>
        protected string OnlyElementsOfType { get; set; }

        /// <inheritdoc />
        public abstract IEnumerable<IElementInteraction> GetInteractions(IElement element);

        /// <summary>
        /// Returns whether the given element is relevant for the given interaction handler.
        /// It uses the information from the OnlyElementsOfType instance.
        /// This method can be called by derived classes.
        /// </summary>
        /// <param name="element">Element to be checked</param>
        /// <returns>true, if relevant</returns>
        protected bool IsRelevant(IElement element)
        {
            if (!string.IsNullOrEmpty(OnlyElementsOfType))
            {
                if (!ClassifierMethods.IsDerivedTypeOf(element, OnlyElementsOfType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}