using DatenMeister.Core;
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
                formsPluginState.NewFormModificationPlugins.Add(
                    new NewFormModificationPlugin
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