using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.WebServer.Controller;

public class FormsControllerInternal
{
    private readonly IScopeStorage _scopeStorage;
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly TemporaryExtentFactory _temporaryExtentFactory;
    private readonly TemporaryExtentLogic _temporaryLogic;

    public FormsControllerInternal(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
            
        _temporaryLogic = new TemporaryExtentLogic(workspaceLogic, scopeStorage);
        _temporaryExtentFactory = new TemporaryExtentFactory(_temporaryLogic);
    }

    public IScopeStorage ScopeStorage => _scopeStorage;

    public IWorkspaceLogic WorkspaceLogic => _workspaceLogic;

    public IElement GetInternal(string formUri)
    {
        if (GetItemByUriParameter(WorkspaceNames.WorkspaceManagement, formUri) is not IElement item)
        {
            throw new InvalidOperationException("Form is not found");
        }

        return item;
    }

    public IElement GetObjectFormForItemInternal(string workspaceId, string itemUrl, string? viewMode)
    {
        var item = GetItemByUriParameter(workspaceId, itemUrl);

        var formFactory = new ObjectFormFactory(_workspaceLogic, _scopeStorage);
        var form = formFactory.CreateObjectFormForItem(item,
            new FormFactoryConfiguration
            {
                ViewModeId = viewMode ?? string.Empty,
                Factory = _temporaryExtentFactory
            });

        if (form == null)
        {
            throw new InvalidOperationException("Form is not defined");
        }

        return form;
    }

    public IElement GetCollectionFormForExtentInternal(string workspaceId, string extentUri, string? viewMode)
    {
        var (collection, extent) = _workspaceLogic.FindExtentAndCollection(workspaceId, extentUri);
        if (collection == null || extent == null)
        {
            throw new InvalidOperationException("Extent not found: " + extentUri);
        }
            
        var formFactory = new FormFactory(_workspaceLogic, _scopeStorage);
        var form = formFactory
            .CreateCollectionFormForExtent(extent,
                new FormFactoryConfiguration
                {
                    ViewModeId = viewMode ?? string.Empty,
                    Factory = _temporaryExtentFactory
                });
            
        if (form == null)
        {
            throw new InvalidOperationException("Form is not defined");
        }
            
        _temporaryLogic.TemporaryExtent.elements().add(form);

        return form;
    }

    /// <summary>
    /// Gets a form from the metaClass. This method is used to create new items
    /// </summary>
    /// <param name="metaClass">MetaClass to be given</param>
    /// <param name="viewMode">The view mode</param>
    /// <returns>The found form</returns>
    public IObject GetObjectFormForMetaClassInternal(string? metaClass, string? viewMode = null)
    {
        var formFactory = new ObjectFormFactory(_workspaceLogic, _scopeStorage);

        var configurationMode = new FormFactoryConfiguration
        {
            ViewModeId = viewMode ?? string.Empty,
            Factory = _temporaryExtentFactory
        };

        IElement? resolvedMetaClass = null;
        if (!string.IsNullOrEmpty(metaClass))
        {
            resolvedMetaClass = _workspaceLogic.Resolve(metaClass, ResolveType.OnlyMetaWorkspaces) as IElement;
            if (resolvedMetaClass == null)
            {
                throw new InvalidOperationException("MetaClass for Form Creation is not found: " + metaClass);
            }
        }

        IElement? form;
        if (resolvedMetaClass == null)
        {
            form = formFactory.CreateEmptyObjectForm(configurationMode);
        }
        else
        {
            form = formFactory.CreateObjectFormForMetaClass(
                resolvedMetaClass,
                configurationMode);
        }

        if (form == null)
        {
            throw new InvalidOperationException("Form is not defined");
        }

        return form;
    }

    /// <summary>
    /// Gets the items by the uri parameter.
    /// The parameter themselves are expected to be uri-encoded, so a decoding via HttpUtility.UrlDecode will be performed
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="itemUri">Uri of the item</param>
    /// <returns>The found object</returns>
    private IObject GetItemByUriParameter(string workspaceId, string itemUri)
    {
        var workspace = _workspaceLogic.GetWorkspace(workspaceId);
        if (workspace == null)
        {
            throw new InvalidOperationException("Extent is not found");
        }

        if (workspace.Resolve(itemUri, ResolveType.NoMetaWorkspaces) is not IObject foundElement)
        {
            throw new InvalidOperationException("Element is not found");
        }

        return foundElement;
    }

    public IEnumerable<IObject> GetViewModesInternal()
    {
        var formsController = new FormMethods(_workspaceLogic, _scopeStorage);
        return formsController.GetViewModes();
    }
}