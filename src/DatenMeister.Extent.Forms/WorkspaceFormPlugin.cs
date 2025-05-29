using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms;

public class WorkspaceFormPlugin: IDatenMeisterPlugin
{
    private readonly IScopeStorage _scopeStorage;

    /// <summary>
    /// Defines the name of the action to create a new extent in the workspace
    /// </summary>
    public const string WorkspaceCreateXmiExtent = "Workspace.Extent.Xmi.Create";
    /// <summary>
    /// Defines the name of the action to create a new extent in the workspace
    /// </summary>
    public const string WorkspaceCreateXmiExtentNavigate = "Workspace.Extent.Xmi.Create.Navigate";
        
    /// <summary>
    /// Defines the name of the action to create a new extent in the workspace
    /// </summary>
    public const string WorkspaceCreateExtentNavigate = "Workspace.Extent.LoadOrCreate.Navigate";


    public WorkspaceFormPlugin(IScopeStorage scopeStorage)
    {
        _scopeStorage = scopeStorage;
    }
    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:
                    
                var formsPlugin = _scopeStorage.Get<FormsPluginState>();
  
                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(WorkspaceCreateExtentNavigate, "Create or Load Extent")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Row,
                        MetaClass = _DatenMeister.TheOne.Management.__Workspace,
                        OnCallSuccess = (element, parameter) =>
                        {
                            // Sets the parameter that the right workspace is used
                            var workspaceId = element?.getOrDefault<string>(_DatenMeister._Management._Workspace.id);
                            if (!string.IsNullOrEmpty(workspaceId))
                            {
                                parameter.Parameter["workspaceId"] = workspaceId;
                            }
                        }
                    });
                    
                ActionButtonToFormAdder.AddActionButton(
                    formsPlugin, new ActionButtonAdderParameter(WorkspaceCreateXmiExtentNavigate, "Create Xmi-Extent")
                    {
                        FormType = _DatenMeister._Forms.___FormType.Row,
                        MetaClass = _DatenMeister.TheOne.Management.__Workspace,
                        OnCallSuccess = (element, parameter) =>
                        {
                            // Sets the parameter that the right workspace is used
                            var workspaceId = element?.getOrDefault<string>(_DatenMeister._Management._Workspace.id);
                            if (!string.IsNullOrEmpty(workspaceId))
                            {
                                parameter.Parameter["workspaceId"] = workspaceId;
                            }
                        }
                    });

                break;
        }

        return Task.CompletedTask;
    }
}