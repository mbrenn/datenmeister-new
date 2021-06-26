using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.WebServer.Models;

namespace DatenMeister.WebServer.InterfaceController
{
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
                    new ExtentCreator(_workspaceLogic, _scopeStorage),
                    _scopeStorage);
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
            // Finds the specific items of the given extent
            IObject? foundElement;
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUrl) as IUriExtent;
            if (extent == null)
            {
                return null;
            }

            foundElement = extent;

            if (!string.IsNullOrEmpty(itemId))
            {
                foundElement = extent.element($"#{itemId}");
            }
            
            if (foundElement == null)
            {
                return null;
            }

            // Create the result 
            var result = new ItemAndFormModel();
            
            // Find the matching form
            var extentForm = _formsPlugin.GetItemTreeFormForObject(
                foundElement,
                FormDefinitionMode.Default,
                "Default");
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
        }

        /// <summary>
        /// Gets the items and forms
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent</param>
        /// <returns>ID of the item</returns>
        public CollectionAndFormModel? GetItemsAndFormOfExtent(string workspaceId, string extentUrl)
        {
            // Finds the specific items of the given extent
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUrl) as IUriExtent;
            if (extent == null)
            {
                return null;
            }

            // Create the result 
            var result = new CollectionAndFormModel();

            // Find the matching form
            var extentForm = _formsPlugin.GetExtentForm(
                extent,
                FormDefinitionMode.Default);
            if (extentForm == null)
            {
                return null;
            }

            result.form = XmiHelper.ConvertToXmiFromObject(extentForm);

            result.items = XmiHelper.ConvertToXmiFromCollection(
                extent.elements());

            // Returns the result
            return result;
        }
    }
}