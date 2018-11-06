using System;

namespace DatenMeister.Core.Plugins
{
    /// <summary>
    /// Defines the loadingPosition point of the plugin. It may be at start after having defined all objects and resolvement or it may be at the end of DatenMeisterPreparation
    /// </summary>
    [Flags]
    public enum PluginLoadingPosition
    {
        /// <summary>
        /// Before the MOF and XMI are boot strapped. This position is used to add new extent types
        /// </summary>
        BeforeBootstrapping = 1,

        /// <summary>
        /// After MOF and XMI are boot strapped. This position is used to define new objects
        /// </summary>
        AfterBootstrapping = 2,

        /// <summary>
        /// After initialization of the AfterBootstrapping has occured. After that event, some internal objects are created upon some DatenMeister internal states
        /// </summary>
        AfterInitialization = 4
    }

    /// <summary>
    /// Defines a dependency between two plugins. The plugin being dependent that the other plugin has been loaded
    /// needs to attach this attribute upon the Plugin class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class PluginDependencyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type upon which the plugin is dependent upon
        /// </summary>
        public Type DependentType { get; }

        /// <summary>
        /// Initializes a new instance of the PluginDependency attribute class
        /// </summary>
        /// <param name="dependentType">Defines teh type upon which this plugin is dependent upon</param>
        /// <param name="pluginEntry">Defines the plugin execution position</param>
        public PluginDependencyAttribute(Type dependentType)
        {
            DependentType = dependentType;
        }
    }

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