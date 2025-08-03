namespace DatenMeister.Plugins;

/// <summary>
///     Defines a dependency between two plugins. The plugin being dependent that the other plugin has been loaded
///     needs to attach this attribute upon the Plugin class
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class PluginDependencyAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the PluginDependency attribute class
    /// </summary>
    /// <param name="dependentType">Defines teh type upon which this plugin is dependent upon</param>
    public PluginDependencyAttribute(Type dependentType)
    {
        DependentType = dependentType;
    }

    /// <summary>
    ///     Gets or sets the type upon which the plugin is dependent upon
    /// </summary>
    public Type DependentType { get; }
}