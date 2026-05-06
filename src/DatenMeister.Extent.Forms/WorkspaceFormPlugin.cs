using System.Web;
using DatenMeister.Actions.ClientActions;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
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

                var excelOneTime =
                    new ActionButtonAdderParameterForRow(NavigationClientActions.ToUrlActionName,
                        "One-Time")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };
                SetCallBackToUrl(excelOneTime, _ExtentLoaderConfigs.TheOne.__ExcelConvertToXmiOnceConfig.Uri);
                var excelReadOnly =
                    new ActionButtonAdderParameterForRow(NavigationClientActions.ToUrlActionName,
                        "Read-Only")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };
                SetCallBackToUrl(excelReadOnly, _ExtentLoaderConfigs.TheOne.__ExcelReadOnlyLoaderConfig.Uri);
                var excelSynchronized =
                    new ActionButtonAdderParameterForRow(NavigationClientActions.ToUrlActionName,
                        "Synchronized")
                    {
                        PredicateForParameter = x => x.MetaClass?.equals(_Management.TheOne.__Workspace) == true
                    };
                SetCallBackToUrl(excelSynchronized, _ExtentLoaderConfigs.TheOne.__ExcelFullSyncLoaderConfig.Uri);

                ActionButtonToFormAdder.AddRowActionButtons(
                    formsState,
                    new ActionButtonToFormAdder.MultiActionButtonsConfig()
                    {
                        Name = "Import Excel",
                        Title = "Import Excel"
                    },
                    [excelReadOnly, excelSynchronized, excelOneTime]);

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
        
        void SetCallBackToUrl(ActionButtonAdderParameterForRow actionParameter, string metaClassUri)
        {
            actionParameter.OnCallSuccess = parameter =>
            {
                // Sets the parameter that the right workspace is used
                var workspaceId =
                    parameter.Element?.getOrDefault<string>(_Management._Workspace.id);
                if (!string.IsNullOrEmpty(workspaceId))
                {
                    actionParameter.Parameter[_Actions._ClientActions._NavigateToUrlClientAction.url] = 
                        "ItemAction/Workspace.Extent.LoadOrCreate.Step2?metaClass=" +
                        HttpUtility.UrlEncode(metaClassUri) +
                        "&workspaceId=" + HttpUtility.UrlEncode(workspaceId);
                }
            };
        }
    }
}