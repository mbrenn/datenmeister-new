using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Verifier;

[PluginLoading(PluginLoadingPosition.BeforeBootstrapping | PluginLoadingPosition.AfterLoadingOfExtents)]
// ReSharper disable once UnusedType.Global
public class VerifierPlugin : IDatenMeisterPlugin
{
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly IScopeStorage _scopeStorage;
    private Verifier? _verifier;

    public VerifierPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
    }
    
    public void Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.BeforeBootstrapping:
                _verifier = new Verifier(_workspaceLogic, _scopeStorage);
                Initializer.InitWithDefaultVerifiers(_workspaceLogic, _verifier);
                _scopeStorage.Add(_verifier);
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
    }
}