using System.Diagnostics;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.WebServer.Models;

namespace DatenMeister.WebServer.InterfaceController;

public class ExtentItemsController
{
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly IScopeStorage _scopeStorage;
    private readonly FormsPlugin _formsPlugin;
        
    public ExtentItemsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
        _formsPlugin =
            new FormsPlugin(
                _workspaceLogic,
                new ExtentCreator(_workspaceLogic, scopeStorage),
                scopeStorage);
    }

    /// <summary>
    /// Gets the items and forms
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="extentUrl">Url of the extent</param>
    /// <param name="itemId">Id of te item</param>
    /// <returns>ID of the item</returns>
    public ItemAndFormModel? GetItemAndForm(string workspaceId, string extentUrl, string? itemId)
    {
        throw new NotImplementedException("GetItemAndForm is not supported anymore");
        // Check, if we are used at all. I would like to get rid of the class.
/*
        Debugger.Break();
        
        // Finds the specific items of the given extent
        var extent = _workspaceLogic.FindExtent(workspaceId, extentUrl) as IUriExtent;
        if (extent == null)
        {
            return null;
        }

        IObject? foundElement = extent;

        if (!string.IsNullOrEmpty(itemId) )
        {
            foundElement = 
                extent.element(
                    itemId.Contains('#') ? itemId : $"#{itemId}");
        }
            
        if (foundElement == null)
        {
            return null;
        }

        // Create the result 
        var result = new ItemAndFormModel();
            
        var objectFormFactory = new ObjectFormFactory(_workspaceLogic, _scopeStorage);
        // Find the matching form
        var extentForm = objectFormFactory.CreateObjectFormForItem(
            foundElement,
            new FormFactoryContext{ViewModeId = "Default"});
            
        if (extentForm == null)
        {
            return null;
        }

        result.form = XmiHelper.ConvertToXmiFromObject(extentForm);
        result.item = XmiHelper.ConvertToXmiFromObject(foundElement);
        result.workspace = workspaceId;
        result.extentUri = extentUrl;
        result.fullName = NamedElementMethods.GetFullName(foundElement);
            
            
        // Returns the result
        return result;
*/
    }

    /// <summary>
    /// Gets the items and forms
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="extentUrl">Url of the extent</param>
    /// <param name="viewMode">View Mode to be used</param>
    /// <returns>ID of the item</returns>
    public CollectionAndFormModel? GetItemsAndFormOfExtent(string workspaceId, string extentUrl, string? viewMode = null)
    {
        throw new NotImplementedException("GetItemsAndFormOfExtent not implemented anymore");
        
        /*
        // Check, if we are used at all. I would like to get rid of the class.
        Debugger.Break();
        
        // Finds the specific items of the given extent
        var extent = _workspaceLogic.FindExtent(workspaceId, extentUrl) as IUriExtent;
        if (extent == null)
        {
            return null;
        }

        // Create the result 
        var result = new CollectionAndFormModel();

        // Find the matching form
        var formFactory = new CollectionFormFactory(_workspaceLogic, _scopeStorage);
        var extentForm = formFactory.CreateCollectionFormForExtent(
            extent,
            new FormFactoryConfiguration { ViewModeId = viewMode ?? ViewModes.Default });
        if (extentForm == null)
        {
            return null;
        }

        result.form = XmiHelper.ConvertToXmiFromObject(extentForm);

        result.items = XmiHelper.ConvertToXmiFromCollection(
            extent.elements());

        // Returns the result
        return result;
        */
    }
}