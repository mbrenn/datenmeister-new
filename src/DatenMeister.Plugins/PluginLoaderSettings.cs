namespace DatenMeister.Plugins;

public class PluginLoaderSettings
{
    /// <summary>
    ///     Gets or sets the plugin loader to be used for the DatenMeister... If none is specified, the default loader will be
    ///     used.
    /// </summary>
    public IPluginLoader PluginLoader { get; set; } = new DefaultPluginLoader();
}