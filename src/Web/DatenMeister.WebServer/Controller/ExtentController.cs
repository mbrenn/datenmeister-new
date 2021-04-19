using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DependencyInjection;
using DatenMeister.ExtentManager.ExtentStorage;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Provider.XMI;
using DatenMeister.WebServer.Models;

namespace DatenMeister.WebServer.Controller
{
    public class ExtentController
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ScopeStorage _scopeStorage;
        private readonly FormsPlugin _formsPlugin;

        public ExtentController(IWorkspaceLogic workspaceLogic, ScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            _formsPlugin =
                new FormsPlugin(
                    _workspaceLogic,
                    new ExtentCreator(_workspaceLogic, _scopeStorage),
                    _scopeStorage);
        }

        public ItemsOverviewModel GetItems(string workspaceId, string extentUrl)
        {
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUrl) as IUriExtent;
            if (extent == null)
            {
                return null;
            }

            var result = new ItemsOverviewModel();
            var extentForm = _formsPlugin.GetExtentForm(
                extent,
                FormDefinitionMode.Default);

            return result;
        }
    }
}