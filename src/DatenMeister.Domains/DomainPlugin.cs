using DatenMeister.Plugins;

namespace DatenMeister.Domains;

/// <summary>
/// Defines the domain Plugin which is used to load the Types and Management Information. 
/// </summary>
public class DomainPlugin : IDatenMeisterPlugin {
    
    public async Task Start(PluginLoadingPosition position)
    {
        await Task.CompletedTask;
    }
}