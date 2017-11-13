namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Defines possible types of the list view control type
    /// </summary>
    public enum ViewDefinitionMode
    {
        /// <summary>
        /// Uses the viewfinder to retrieve the view
        /// </summary> 
        Default,

        /// <summary>
        /// Returns all properties by form creator
        /// </summary>
        AllProperties,

        /// <summary>
        /// Uses the view as given in the instance
        /// </summary>
        Specific
    }
}