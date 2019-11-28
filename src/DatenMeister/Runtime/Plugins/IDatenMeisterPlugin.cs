namespace DatenMeister.Runtime.Plugins
{
    /// <summary>
    /// Defines the interface for the plugins.
    /// The corresponding types implementing this interface are activated by the dependency injection
    /// </summary>
    public interface IDatenMeisterPlugin
    {
        /// <summary>
        /// Starts the plugin
        /// </summary>
        void Start(PluginLoadingPosition position);
    }
}