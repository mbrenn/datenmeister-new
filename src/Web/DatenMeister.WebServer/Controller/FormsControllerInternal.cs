using System;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms;

namespace DatenMeister.WebServer.Controller
{
    public class FormsControllerInternal
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public FormsControllerInternal(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public IElement GetInternal(string formUri)
        {
            formUri = HttpUtility.UrlDecode(formUri);
            if (GetItemByUriParameter(WorkspaceNames.WorkspaceManagement, formUri) is not IElement item)
            {
                throw new InvalidOperationException("Form is not found");
            }

            return item;
        }

        public IElement GetDefaultFormForItemInternal(string workspaceId, string itemUrl, string? viewMode)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUrl = HttpUtility.UrlDecode(itemUrl);
            viewMode = HttpUtility.UrlDecode(viewMode);

            var item = GetItemByUriParameter(workspaceId, itemUrl);

            var formLogic = new FormsPlugin(_workspaceLogic, _scopeStorage);
            var formFactory = new FormFactory(formLogic, _scopeStorage);
            var form = formFactory.CreateExtentFormForItem(item,
                new FormFactoryConfiguration {ViewModeId = viewMode ?? string.Empty});

            if (form == null)
            {
                throw new InvalidOperationException("Form is not defined");
            }

            return form;
        }

        public IElement GetDefaultFormForExtentInternal(string workspaceId, string extentUri, string? viewMode)
        {
            viewMode = HttpUtility.UrlDecode(viewMode);
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);

            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri)
                         ?? throw new InvalidOperationException("Extent is not found");

            var formLogic = new FormsPlugin(_workspaceLogic, _scopeStorage);
            var formFactory = new FormFactory(formLogic, _scopeStorage);
            var form = formFactory.CreateExtentFormForExtent(extent,
                new FormFactoryConfiguration {ViewModeId = viewMode ?? string.Empty});
            if (form == null)
            {
                throw new InvalidOperationException("Form is not defined");
            }

            return form;
        }

        /// <summary>
        /// Gets a form from the metaClass. This method is used to create new items
        /// </summary>
        /// <param name="metaClass">MetaClass to be given</param>
        /// <param name="viewMode">The view mode</param>
        /// <returns>The found form</returns>
        public IObject GetDefaultFormForMetaClassInternal(string metaClass, string? viewMode = null)
        {
            viewMode = HttpUtility.UrlDecode(viewMode);
            metaClass = HttpUtility.UrlDecode(metaClass);

            var formLogic = new FormsPlugin(_workspaceLogic, _scopeStorage);
            var formFactory = new FormFactory(formLogic, _scopeStorage);
            if (
                _workspaceLogic.GetTypesWorkspace().FindElementByUri(metaClass) is not IElement element)
            {
                throw new InvalidOperationException("Element is not found: " + metaClass);
            }

            var form = formFactory.CreateExtentFormForItemsMetaClass(
                element,
                new FormFactoryConfiguration {ViewModeId = viewMode ?? string.Empty});

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
    }
}