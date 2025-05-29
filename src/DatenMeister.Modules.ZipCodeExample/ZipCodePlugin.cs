using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Modules.ZipCodeExample.Forms;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Plugins;
using DatenMeister.Types;

namespace DatenMeister.Modules.ZipCodeExample;

/// <summary>
/// Integrates the zip code example into the DatenMeister framework
/// </summary>
[PluginLoading(
    PluginLoadingPosition.AfterInitialization 
    | PluginLoadingPosition.AfterBootstrapping
    | PluginLoadingPosition.AfterLoadingOfExtents)]
public class ZipCodePlugin : IDatenMeisterPlugin
{
    public const string ZipCodeExtentType = "DatenMeister.Example.ZipCodes";

    public const string CreateZipExample = "ZipExample.CreateExample";

    private static readonly ILogger Logger = new ClassLogger(typeof(ZipCodePlugin));
    private readonly ExtentSettings _extentSettings;

    private readonly LocalTypeSupport _localTypeSupport;
    private readonly IScopeStorage _scopeStorage;

    /// <summary>
    /// Initializes a new instance of the ZipCodePlugin
    /// </summary>
    /// <param name="localTypeSupport">The local type support being used</param>
    /// <param name="scopeStorage">Scope storage</param>
    public ZipCodePlugin(
        LocalTypeSupport localTypeSupport,
        IScopeStorage scopeStorage
    )
    {
        _localTypeSupport = localTypeSupport;
        _scopeStorage = scopeStorage;
        _extentSettings = scopeStorage.Get<ExtentSettings>();
    }

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterInitialization:
            {
                Logger.Info("Initializing type for ZipCode-Plugin");
                var zipCodeModel = _scopeStorage.Get<ZipCodeModel>();

                // Load Resource
                var types = _localTypeSupport.AddInternalTypes(
                    ZipCodeModel.PackagePath,
                    new[] {typeof(ZipCode), typeof(ZipCodeWithState)});
                zipCodeModel.ZipCode = types.ElementAt(0);
                zipCodeModel.ZipCodeWithState = types.ElementAt(1);

                if (zipCodeModel.ZipCode == null || zipCodeModel.ZipCodeWithState == null)
                    throw new InvalidOperationException("The ZipCode Model could not be created");

                ActionButtonToFormAdder.AddActionButton(
                    _scopeStorage.Get<FormsPluginState>(),
                    new ActionButtonAdderParameter(CreateZipExample, "Create Zip-Example")
                    {
                        MetaClass = _DatenMeister.TheOne.Management.__Workspace,
                        FormType = _DatenMeister._Forms.___FormType.Object,
                        PredicateForElement = 
                            element => 
                                element?.getOrDefault<string>(
                                    _DatenMeister._Management._Workspace.id) == WorkspaceNames.WorkspaceData
                    });

                break;
            }
            case PluginLoadingPosition.AfterBootstrapping:
                _extentSettings.extentTypeSettings.Add(
                    new ExtentType(ZipCodeExtentType));
                break;
                
            case PluginLoadingPosition.AfterLoadingOfExtents:
                // Loads the Zipcode Form Modification Plugin in which the user may directly create an zip
                // code example in a workspace object
                var formsPluginState = _scopeStorage.Get<FormsPluginState>();
                formsPluginState.FormModificationPlugins.Add(
                    new ZipCodeFormModificationPlugin());
                break;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Creates a new instance of the ZipCodePlugin by Workspace Logic and Scope STorage
    /// </summary>
    /// <param name="workspaceLogic">Workspacelogic to be used</param>
    /// <param name="scopeStorage">Scope storage to be used</param>
    /// <returns>New instance of the</returns>
    public static ZipCodePlugin Create(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        var localTypeSupport = new LocalTypeSupport(workspaceLogic, scopeStorage);
        return new ZipCodePlugin(localTypeSupport, scopeStorage);
    }
}