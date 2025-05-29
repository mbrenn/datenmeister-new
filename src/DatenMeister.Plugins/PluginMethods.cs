namespace DatenMeister.Plugins;

public static class PluginMethods
{
    /// <summary>
    ///     Calls every plugin loading stage.
    ///     This method can be used for testing to initialize the plugins
    /// </summary>
    /// <param name="datenMeisterPlugin">The plugin to be executed</param>
    public static void StartThrough(this IDatenMeisterPlugin datenMeisterPlugin)
    {
        datenMeisterPlugin.Start(PluginLoadingPosition.BeforeBootstrapping);
        datenMeisterPlugin.Start(PluginLoadingPosition.AfterBootstrapping);
        datenMeisterPlugin.Start(PluginLoadingPosition.AfterInitialization);
        datenMeisterPlugin.Start(PluginLoadingPosition.AfterLoadingOfExtents);
    }

    /// <summary>
    ///     Calls every plugin loading stage.
    ///     This method can be used for testing to initialize the plugins.
    ///     The phases are maintained that means that all plugins are first called in the first phase,
    ///     afterwards all plugins are called in the second phase
    /// </summary>
    /// <param name="datenMeisterPlugins">The plugins to be executed</param>
    public static void StartThrough(this IEnumerable<IDatenMeisterPlugin> datenMeisterPlugins)
    {
        var list = datenMeisterPlugins.ToList();
        foreach (var datenMeisterPlugin in list)
            datenMeisterPlugin.Start(PluginLoadingPosition.BeforeBootstrapping);

        foreach (var datenMeisterPlugin in list) datenMeisterPlugin.Start(PluginLoadingPosition.AfterBootstrapping);

        foreach (var datenMeisterPlugin in list)
            datenMeisterPlugin.Start(PluginLoadingPosition.AfterInitialization);

        foreach (var datenMeisterPlugin in list)
            datenMeisterPlugin.Start(PluginLoadingPosition.AfterLoadingOfExtents);
    }
}