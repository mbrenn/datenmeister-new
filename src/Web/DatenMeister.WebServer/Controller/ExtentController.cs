using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.ExtentManager.ExtentStorage;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.WebServer.Models;

namespace DatenMeister.WebServer.Controller
{
    public class ExtentController
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;
        private readonly FormsPlugin _formsPlugin;

        public ExtentController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
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
        public ItemsOverviewModel? GetItemsAndForm(string workspaceId, string extentUrl, string? itemId)
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
            var result = new ItemsOverviewModel();
            
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

            if (foundElement is IExtent asExtent)
            {
                result.items = XmiHelper.ConvertToXmiFromCollection(
                    asExtent.elements());
            }
            else
            {
                result.items = XmiHelper.ConvertToXmiFromCollection(
                    DefaultClassifierHints.GetPackagedElements(foundElement));
            }
           
            // Gets the items
            
            
            // Returns the result
            return result;
        }
    }
}