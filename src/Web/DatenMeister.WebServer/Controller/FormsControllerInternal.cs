using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
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

    public IElement GetObjectFormForItemInternal(string workspaceId, string itemUrl, string viewMode)
    {
        var item = GetItemByUriParameter(workspaceId, itemUrl);
        var factory = new FormCreationContextFactory(WorkspaceLogic, ScopeStorage)
        {
            MofFactory = _temporaryExtentFactory
        };

        var formContext = factory.Create(viewMode);

        var parameter = new ObjectFormFactoryParameter
        {
            Element = item
        };
        parameter.SetByExtent(item.GetExtentOf() as IUriExtent);
        
        var form = FormCreation.CreateObjectForm(
            parameter, formContext);

        if (form.Form == null)
        {
            throw new InvalidOperationException("Form is not defined");
        }

        return form.Form;
    }

    public IElement GetCollectionFormForExtentInternal(string workspaceId, string extentUri, string viewMode)
    {
        var (collection, extent) = WorkspaceLogic.FindExtentAndCollection(workspaceId, extentUri);
        if (collection == null || extent == null)
        {
            throw new InvalidOperationException("Extent not found: " + extentUri);
        }

        var factory = new FormCreationContextFactory(WorkspaceLogic, ScopeStorage)
        {
            MofFactory = _temporaryExtentFactory
        };
        
        var formContext = factory.Create(viewMode);

        var form = FormCreation.CreateCollectionForm(
            new CollectionFormFactoryParameter
            {
                Collection = extent.elements(),
                Extent = extent,
                ExtentTypes = extent.GetConfiguration().ExtentTypes
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
    public IObject GetObjectFormForMetaClassInternal(string? metaClass, string viewMode)
    {
        var factory = new FormCreationContextFactory(WorkspaceLogic, ScopeStorage)
        {
            MofFactory = _temporaryExtentFactory
        };

        var context = factory.Create(viewMode);

        IElement? resolvedMetaClass = null;
        if (!string.IsNullOrEmpty(metaClass))
        {
            resolvedMetaClass = WorkspaceLogic.Resolve(metaClass, ResolveType.IncludeMetaWorkspaces) as IElement;
            if (resolvedMetaClass == null)
            {
                throw new InvalidOperationException("MetaClass for Form Creation is not found: " + metaClass);
            }
        }
        
        var result = FormCreation.CreateObjectForm(
            new ObjectFormFactoryParameter
            {
                MetaClass = resolvedMetaClass
            }, context);

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

        if (workspace.Resolve(itemUri, ResolveType.IncludeWorkspace) is not IObject foundElement)
        {
            throw new InvalidOperationException("Element is not found");
        }

        return foundElement;
    }

    public IEnumerable<IObject> GetViewModesInternal()
    {
        var viewModeMethods = new ViewModeMethods(WorkspaceLogic);
        return viewModeMethods.GetViewModes();
    }
}