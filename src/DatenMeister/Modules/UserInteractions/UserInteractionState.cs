using System.Collections.Generic;

namespace DatenMeister.Modules.UserInteractions
{
    /// <summary>
    /// Stores the information of the userinteraction module
    /// </summary>
    public class UserInteractionState
    {
        /// <summary>
        /// Gets the element interaction handlers
        /// </summary>
        public List<IElementInteractionsHandler> ElementInteractionHandler { get; }
            = new List<IElementInteractionsHandler>();
    }
}