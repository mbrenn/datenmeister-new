namespace DatenMeister.Core.Provider.Interfaces
{
    /// <summary>
    /// Defines the possible extent creation methods for the extent manager.
    /// Per default, the option load-only is chosen per default
    /// </summary>
    public enum ExtentCreationFlags
    {
        /// <summary>
        /// Tries to load the extent. If the extent cannot be loaded, the extent is NOT created
        /// </summary>
        LoadOnly,

        /// <summary>
        /// Tries to load the extent. If the extent cannot be loaded, a new extent is created
        /// </summary>
        LoadOrCreate,

        /// <summary>
        /// Creates a new extent by overwriting the existing one
        /// </summary>
        CreateOnly
    }
}