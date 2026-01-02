namespace DatenMeister.Plugins;

/// <summary>
/// Defines the trigger when the plugin shall be called.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class PluginLoadingAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the PluginDependency attribute class
    /// </summary>
    /// <param name="pluginLoadingPosition">Defines the plugin execution position</param>
    public PluginLoadingAttribute(PluginLoadingPosition pluginLoadingPosition)
    {
        PluginLoadingPosition = pluginLoadingPosition;
    }

    public PluginLoadingPosition PluginLoadingPosition { get; }
}