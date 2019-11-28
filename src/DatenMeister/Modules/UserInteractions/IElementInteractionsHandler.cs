using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.UserInteractions
{
    /// <summary>
    /// Defines an interface collecting user interactions for a specific element
    /// </summary>
    public interface IElementInteractionsHandler
    {
        /// <summary>
        /// Gets the interactions that can be performed on the given element
        /// </summary>
        /// <param name="element">Element on which the interactions can be performed</param>
        /// <returns>Enumeration of possible interactions</returns>
        IEnumerable<IElementInteraction> GetInteractions(IObject element);
    }
}