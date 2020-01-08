#nullable enable 

namespace DatenMeister.Modules.Forms.FormFinder
{
    /// <summary>
    /// Defines possible types of the list view control type
    /// </summary>
    public enum FormDefinitionMode
    {
        /// <summary>
        /// Uses the viewfinder to retrieve the view
        /// </summary>
        Default = ViaFormCreator | ViaFormFinder,

        /// <summary>
        /// Returns all properties by form creator
        /// </summary>
        ViaFormCreator = 0x01,

        /// <summary>
        /// Allows the finding of a form via the view finder
        /// </summary>
        ViaFormFinder = 0x02,

        /// <summary>
        /// No automatic creation is defined and the caller has to set the corresponding form to be used
        /// </summary>
        Specific = 0x04
    }
}