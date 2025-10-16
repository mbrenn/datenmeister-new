using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Verifier;

[PluginLoading(PluginLoadingPosition.BeforeBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
// ReSharper disable once UnusedType.Global
public class VerifierPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    private Verifier? _verifier;

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.BeforeBootstrapping:
                _verifier = new Verifier(workspaceLogic, scopeStorage);
                Initializer.InitWithDefaultVerifiers(workspaceLogic, _verifier);
                scopeStorage.Add(_verifier);
                break;
            case PluginLoadingPosition.AfterLoadingOfExtents:
                Task.Run(async () =>
                {
                    _ =_verifier ?? throw new InvalidOperationException("Verifier is not set");

                    await Task.Delay(1000);
                    await _verifier.VerifyExtents();
                });
                break;
        }

        return Task.CompletedTask;
    }
}