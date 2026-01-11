using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Domains.Model;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms.Helper;
using DatenMeister.Types.Plugin;

namespace DatenMeister.Domains;

public class DomainCreateFoundationActionHandler(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) 
    : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Root.TheOne.__DomainCreateFoundationAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var extentManager = new ExtentManager(workspaceLogic, scopeStorage);
        var domainCreate = new Root.DomainCreateFoundationAction_Wrapper(action);
        
        // Cleans up the pre- and postfixes
        var prefix = domainCreate.extentUriPrefix ?? string.Empty;
        var postfix = domainCreate.extentUriPostfix ?? string.Empty;
        if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith('.'))
        {
            prefix += ".";
        }

        if (!string.IsNullOrEmpty(postfix) && !postfix.StartsWith('.'))
        {
            postfix = "." + postfix;
        }
        
        var filePathPrefix = 
            Path.Combine(domainCreate.filePath ?? string.Empty, domainCreate.name ?? string.Empty);

        // We have to do the following tasks
        // 1. Create an empty Xmi-Extent into the specified folder with the name {Domain}_Management
        // 1.1 Set Extent Type to FormMethods.FormExtentType = "DatenMeister.Forms";
        // 2. Create an empty Xmi-Extent into the specified folder with the name {Domain}_Types
        // 2.1. Set Extent Type to Uml.Classes (UmlPlugin.ExtentType)
        // 3. If required, add an empty Xmi-Extent into the specified folder with name {Domain_Data}

        // 1.
        var managementConfiguration =
            new ExtentLoaderConfigs.XmiStorageLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
                {
                    workspaceId = WorkspaceNames.WorkspaceManagement,
                    filePath = filePathPrefix + "_Management.xmi",
                    extentUri = $"dm:///{prefix}management.{domainCreate.name}{postfix}"
                };

        var loadedManagementExtent = 
            await extentManager.LoadExtent(managementConfiguration.GetWrappedElement(), ExtentCreationFlags.LoadOrCreate);
        if (loadedManagementExtent.LoadingState == ExtentLoadingState.Failed
            || loadedManagementExtent.Extent == null)
        {
            throw new InvalidOperationException("Management-Extent could not be loaded: " + loadedManagementExtent.FailLoadingMessage);
        }

        // 1.1
        loadedManagementExtent.Extent.GetConfiguration().ExtentType = FormMethods.FormExtentType;
        
        // 2.
        var typesConfiguration =
            new ExtentLoaderConfigs.XmiStorageLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
                {
                    workspaceId = WorkspaceNames.WorkspaceTypes,
                    filePath = filePathPrefix + "_Types.xmi",
                    extentUri = $"dm:///{prefix}types.{domainCreate.name}{postfix}"
                };
        var loadedTypesExtent = 
            await extentManager.LoadExtent(typesConfiguration.GetWrappedElement(), ExtentCreationFlags.LoadOrCreate);
        if (loadedTypesExtent.LoadingState == ExtentLoadingState.Failed 
            || loadedTypesExtent.Extent == null)
        {
            throw new InvalidOperationException("Types-Extent could not be loaded: " + loadedTypesExtent.FailLoadingMessage);
        }
        
        // 2.1
        loadedTypesExtent.Extent.GetConfiguration().ExtentType = UmlPlugin.ExtentType;
        
        // 3.
        if (domainCreate.createDataExtent)
        {
            var dataConfiguration =
                new ExtentLoaderConfigs.XmiStorageLoaderConfig_Wrapper(InMemoryObject.TemporaryFactory)
                    {
                        workspaceId = WorkspaceNames.WorkspaceData,
                        filePath = filePathPrefix + "_Data.xmi",
                        extentUri = $"dm:///{prefix}data.{domainCreate.name}{postfix}"
                    };
            var loadedDataExtent =
                await extentManager.LoadExtent(dataConfiguration.GetWrappedElement(), ExtentCreationFlags.LoadOrCreate);
            if (loadedDataExtent.LoadingState == ExtentLoadingState.Failed
                || loadedDataExtent.Extent == null)
            {
                throw new InvalidOperationException("Data-Extent could not be loaded: " +
                                                    loadedDataExtent.FailLoadingMessage);
            }
        }

        return null;
    }
}