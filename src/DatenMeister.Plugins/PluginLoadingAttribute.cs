namespace DatenMeister.Plugins
{
    /// <summary>
    ///     Defines a dependency between two plugins. The plugin being dependent that the other plugin has been loaded
    ///     needs to attach this attribute upon the Plugin class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PluginLoadingAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the PluginDependency attribute class
        /// </summary>
        /// <param name="pluginLoadingPosition">Defines the plugin execution position</param>
        public PluginLoadingAttribute(
            PluginLoadingPosition pluginLoadingPosition = PluginLoadingPosition.AfterLoadingOfExtents)
        {
            PluginLoadingPosition = pluginLoadingPosition;
        }

        public PluginLoadingPosition PluginLoadingPosition { get; }
    }
}