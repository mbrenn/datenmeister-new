namespace DatenMeister.WPF.Modules.ViewExtensions
{
    /// <summary>
    /// Defines the scope of the menu. Only view extensions of the given scope
    /// are added to the given menu
    /// </summary>
    [Flags]
    public enum NavigationScope
    {
        /// <summary>
        /// This item or this menu references to the complete application
        /// </summary>
        Application = 0x01,

        /// <summary>
        /// This item or this menu references to the complete extent
        /// </summary>
        Extent = 0x02,
        
        /// <summary>
        /// This item or this menu references to the a selected item and its collection of the given extent
        /// </summary>
        Collection = 0x04,

        /// <summary>
        /// This item or this menu references to a selected detail item
        /// </summary>
        Item = 0x08
    }
}