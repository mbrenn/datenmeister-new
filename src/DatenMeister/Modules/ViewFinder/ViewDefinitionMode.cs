namespace DatenMeister.Modules.ViewFinder
{
    /// <summary>
    /// Defines possible types of the list view control type
    /// </summary>
    public enum ViewDefinitionMode
    {
        /// <summary>
        /// Uses the viewfinder to retrieve the view
        /// </summary>
        Default = 0x03,

        /// <summary>
        /// Returns all properties by form creator
        /// </summary>
        ViaFormCreator = 0x01,

        /// <summary>
        /// Allows the finding of a form via the view finder
        /// </summary>
        ViaViewFinder = 0x02,

        /// <summary>
        /// No automatic creation is defined and the caller has to set the corresponding form to be used
        /// </summary>
        Specific = 0x04
    }
}