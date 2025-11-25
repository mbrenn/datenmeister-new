using DatenMeister.Core.Interfaces;
using DatenMeister.Forms;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms;

public class DefaultTypesPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                var formsPluginState = scopeStorage.Get<FormsState>();
                formsPluginState.FormModificationPlugins.Add(
                    new FormModificationPlugin
                    {
                        CreateContext =
                            context =>
                                context.Global.ObjectFormFactories.Add(
                                    new PackageFormModificationPlugin()),
                        Name = "PackageFormModificationPlugin"
                    });
                break;
        }

        return Task.CompletedTask;
    }
}