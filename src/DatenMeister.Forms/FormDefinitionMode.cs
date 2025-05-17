namespace DatenMeister.Forms
{
    /// <summary>
    /// Defines possible types of the list view control type
    /// </summary>
    [Flags]
    public enum FormDefinitionMode
    {
        /// <summary>
        /// Uses the viewfinder to retrieve the view
        /// </summary>
        Default = ViaFormCreator | ViaFormFinder,

        /// <summary>
        /// Returns all properties by form reportCreator
        /// </summary>
        ViaFormCreator = 0x01,

        /// <summary>
        /// Allows the finding of a form via the view finder
        /// </summary>
        ViaFormFinder = 0x02,

        /// <summary>
        /// No automatic creation is defined and the caller has to set the corresponding form to be used
        /// </summary>
        Specific = 0x04,

        /// <summary>
        /// Does not allow the modifications via the IFormModificationPlugin
        /// </summary>
        NoFormModifications = 0x08
    }

    public static class FormDefinitionModeExtensions
    {
        public static bool HasFlagFast(this FormDefinitionMode value, FormDefinitionMode flag)
        {
            return (value & flag) != 0;
        }
    }
}