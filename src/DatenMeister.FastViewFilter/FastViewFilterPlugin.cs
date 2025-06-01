using DatenMeister.Plugins;

namespace DatenMeister.FastViewFilter;

/// <summary>
/// Implements the plugin adding the fast views
/// </summary>
[PluginLoading]
// ReSharper disable once UnusedMember.Global
public class FastViewFilterPlugin : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        return Task.CompletedTask;
    }
}