using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.ExtentManager.ExtentStorage;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.UserProperties;
using DatenMeister.Provider.XMI;
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

        public ItemsOverviewModel? GetItems(string workspaceId, string extentUrl, string? itemUrl)
        {
            // Finds the specific items of the given extent
            IObject? foundElement;
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUrl) as IUriExtent;
            if (extent == null)
            {
                return null;
            }

            foundElement = extent;

            if (!string.IsNullOrEmpty(itemUrl))
            {
                foundElement = extent.element(itemUrl);
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

            result.form = XmiHelper.ConvertToXmi(extentForm);
            
            // Gets the items
            result.items = XmiHelper.ConvertToXmi(extent.elements());
            
            // Returns the result
            return result;
        }
    }
}