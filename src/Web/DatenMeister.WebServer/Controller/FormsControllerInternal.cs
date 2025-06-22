using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.WebServer.Controller;

public class FormsControllerInternal
{
    private readonly TemporaryExtentFactory _temporaryExtentFactory;
    private readonly TemporaryExtentLogic _temporaryLogic;
    
    public TemporaryExtentFactory TemporaryExtentFactory => _temporaryExtentFactory;

    public FormsControllerInternal(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        WorkspaceLogic = workspaceLogic;
        ScopeStorage = scopeStorage;
        _temporaryLogic = new TemporaryExtentLogic(workspaceLogic, scopeStorage);
        _temporaryExtentFactory = new TemporaryExtentFactory(_temporaryLogic);
    }

    public IScopeStorage ScopeStorage { get; }

    public IWorkspaceLogic WorkspaceLogic { get; }

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
        var factory = new NewFormCreationContextFactory(WorkspaceLogic, ScopeStorage)
        {
            MofFactory = _temporaryExtentFactory
        };

        var formContext = factory.Create()
            .SetViewModeId(viewMode ?? string.Empty);
        
        var form = FormCreation.CreateObjectFormForItem(item,
            formContext);

        if (form.Form == null)
        {
            throw new InvalidOperationException("Form is not defined");
        }

        return form.Form;
    }

    public IElement GetCollectionFormForExtentInternal(string workspaceId, string extentUri, string? viewMode)
    {
        var (collection, extent) = WorkspaceLogic.FindExtentAndCollection(workspaceId, extentUri);
        if (collection == null || extent == null)
        {
            throw new InvalidOperationException("Extent not found: " + extentUri);
        }

        var factory = new NewFormCreationContextFactory(WorkspaceLogic, ScopeStorage)
        {
            MofFactory = _temporaryExtentFactory
        };
        
        var formContext = factory.Create();

        var form = FormCreation.CreateCollectionForm(
            new CollectionFormFactoryParameter
            {
                Collection = extent.elements()
            },
            formContext);
            
        if (form.Form == null)
        {
            throw new InvalidOperationException("Form is not defined");
        }
            
        _temporaryLogic.TemporaryExtent.elements().add(form);

        return form.Form;
    }

    /// <summary>
    /// Gets a form from the metaClass. This method is used to create new items
    /// </summary>
    /// <param name="metaClass">MetaClass to be given</param>
    /// <param name="viewMode">The view mode</param>
    /// <returns>The found form</returns>
    public IObject GetObjectFormForMetaClassInternal(string? metaClass, string? viewMode = null)
    {
        var factory = new NewFormCreationContextFactory(WorkspaceLogic, ScopeStorage)
        {
            MofFactory = _temporaryExtentFactory
        };

        var context = factory.Create().SetViewModeId(viewMode ?? string.Empty);

        IElement? resolvedMetaClass = null;
        if (!string.IsNullOrEmpty(metaClass))
        {
            resolvedMetaClass = WorkspaceLogic.Resolve(metaClass, ResolveType.OnlyMetaWorkspaces) as IElement;
            if (resolvedMetaClass == null)
            {
                throw new InvalidOperationException("MetaClass for Form Creation is not found: " + metaClass);
            }
        }
        
        var result = FormCreation.CreateObjectFormForMetaClass(
            resolvedMetaClass,
            context);

        if (result.Form == null)
        {
            throw new InvalidOperationException("Form is not defined");
        }

        return result.Form;
    }

    /// <summary>
    /// Gets the items by the uri parameter.
    /// The parameters themselves are expected to be uri-encoded, so a decoding via HttpUtility.UrlDecode will be performed
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="itemUri">Uri of the item</param>
    /// <returns>The found object</returns>
    private IObject GetItemByUriParameter(string workspaceId, string itemUri)
    {
        var workspace = WorkspaceLogic.GetWorkspace(workspaceId);
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
        var formsController = new FormMethods(WorkspaceLogic, ScopeStorage);
        return formsController.GetViewModes();
    }
}