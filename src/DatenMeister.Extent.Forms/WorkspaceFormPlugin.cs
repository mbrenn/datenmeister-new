using DatenMeister.Core.Interfaces;
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
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                var formsState = scopeStorage.Get<FormsState>();

                var actionParameter =
                    new ActionButtonAdderParameterForRow(WorkspaceCreateExtentNavigate, "Create or Load Extent")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true,
                        Priority = FormFactoryPriorities.AdditionalFunctionsPrimary
                    };

                SetCallBack(actionParameter);

                ActionButtonToFormAdder.AddRowActionButton(formsState, actionParameter);

                var otherActionParameter =
                    new ActionButtonAdderParameterForRow(WorkspaceCreateXmiExtentNavigate, "Create Xmi-Extent")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };
                SetCallBack(otherActionParameter);

                ActionButtonToFormAdder.AddRowActionButton(
                    formsState, otherActionParameter);

                var excel1 =
                    new ActionButtonAdderParameterForRow(WorkspaceCreateXmiExtentNavigate,
                        "Create Read-Only Excel-Extent")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };
                SetCallBack(excel1);
                var excel2 =
                    new ActionButtonAdderParameterForRow(WorkspaceCreateXmiExtentNavigate,
                        "Create synchronized Excel-Extent")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };
                SetCallBack(excel2);
                var excel3 =
                    new ActionButtonAdderParameterForRow(WorkspaceCreateXmiExtentNavigate,
                        "Create one-time Import Excel-Extent")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };
                SetCallBack(excel3);

                ActionButtonToFormAdder.AddRowActionButtons(
                    formsState,
                    new ActionButtonToFormAdder.MultiActionButtonsConfig()
                    {
                        Name = "Import Excel",
                        Title = "Import Excel"
                    },
                    [excel1, excel2, excel3]);


                break;
        }

        return Task.CompletedTask;

        void SetCallBack(ActionButtonAdderParameterForRow actionParameter)
        {
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
        }
    }
}