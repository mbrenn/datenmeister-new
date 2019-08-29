using System;

namespace DatenMeister.Core.Plugins
{
    /// <summary>
    /// Defines a dependency between two plugins. The plugin being dependent that the other plugin has been loaded
    /// needs to attach this attribute upon the Plugin class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class PluginLoadingAttribute : Attribute
    {
        public PluginLoadingPosition PluginLoadingPosition { get; }

        /// <summary>
        /// Initializes a new instance of the PluginDependency attribute class
        /// </summary>
        /// <param name="dependentType">Defines teh type upon which this plugin is dependent upon</param>
        /// <param name="pluginLoadingPosition">Defines the plugin execution position</param>
        public PluginLoadingAttribute(PluginLoadingPosition pluginLoadingPosition = PluginLoadingPosition.AfterInitialization)
        {
            PluginLoadingPosition = pluginLoadingPosition;
        }
    }
}