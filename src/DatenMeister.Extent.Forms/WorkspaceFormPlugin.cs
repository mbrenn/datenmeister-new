using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms;

public class WorkspaceFormPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
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


    public Task Start(PluginLoadingPosition position)
    {
        // TODO: Reactivate the constraints
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:
                    
                var formsPlugin = scopeStorage.Get<FormsState>();

                var actionParameter =
                    new ActionButtonAdderParameterForRow(WorkspaceCreateExtentNavigate, "Create or Load Extent")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };

                actionParameter.OnCallSuccess = parameter =>
                {
                    // Sets the parameter that the right workspace is used
                    var workspaceId =
                        parameter.Element?.getOrDefault<string>(_Management._Workspace.id);
                    if (!string.IsNullOrEmpty(workspaceId))
                    {
                        actionParameter.Parameter["workspaceId"] = workspaceId;
                    }
                };
                
                ActionButtonToFormAdder.AddRowActionButton(
                    formsPlugin, actionParameter);
                
                var otherActionParameter =
                    new ActionButtonAdderParameterForRow(WorkspaceCreateExtentNavigate, "Create or Load Extent")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };

                otherActionParameter.OnCallSuccess = parameter =>
                {
                    // Sets the parameter that the right workspace is used
                    var workspaceId =
                        parameter.Element?.getOrDefault<string>(_Management._Workspace.id);
                    if (!string.IsNullOrEmpty(workspaceId))
                    {
                        otherActionParameter.Parameter["workspaceId"] = workspaceId;
                    }
                };
                    
                ActionButtonToFormAdder.AddRowActionButton(
                    formsPlugin, otherActionParameter);

                break;
        }

        return Task.CompletedTask;
    }
}