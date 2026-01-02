using DatenMeister.Plugins;

namespace DatenMeister.Domains;

/// <summary>
/// Defines the domain Plugin which is used to load the Types and Management Information. 
/// </summary>
[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
public class DomainPlugin : IDatenMeisterPlugin {
    
    public async Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                break;
            
        }
        
        await Task.CompletedTask;
    }
}