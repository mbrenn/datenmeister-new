using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Uml.Helper;

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
        protected IElement? OnlyElementsOfType {
            get
            {
                return OnlyElementsOfTypes.Count switch
                {
                    1 => OnlyElementsOfTypes.First(),
                    0 => null,
                    _ => throw new InvalidOperationException("OnlyElementsOfType.Count is not 0 or 1")
                };
            }
            set
            {
                OnlyElementsOfTypes.Clear();
                if (value != null)
                {
                    OnlyElementsOfTypes.Add(value);
                }
            }
        }
        
        protected List<IElement> OnlyElementsOfTypes { get; } = new List<IElement>();

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
            var elementAsElement = element as IElement;
            var metaClassElement = elementAsElement?.getMetaClass();
            if (metaClassElement == null)
            {
                return false;
            }

            foreach (var type in OnlyElementsOfTypes)
            {
                if (ClassifierMethods.IsSpecializedClassifierOf(
                    metaClassElement,
                    type))
                {
                    return true;
                }
            }

            return false;
        }
    }
}