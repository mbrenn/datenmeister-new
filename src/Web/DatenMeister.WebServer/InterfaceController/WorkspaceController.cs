using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.WebServer.Models;

namespace DatenMeister.WebServer.InterfaceController
{
    /// <summary>
    /// This controller supports the export of the workspaces including extents
    /// </summary>
    public class WorkspaceController
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public WorkspaceController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public List<WorkspaceModel> GetWorkspaceModels()
        {
            var result = new List<WorkspaceModel>();
            var workspaces = _workspaceLogic.Workspaces;
            foreach (var workspace in workspaces)
            {
                var workspaceModel = new WorkspaceModel
                {
                    id = workspace.id, 
                    annotation = workspace.annotation
                };

                foreach (var extent in workspace.extent.OfType<IUriExtent>())
                {
                    var extentModel = new ExtentModel
                    {
                        uri = extent.contextURI()
                    };
                    
                    workspaceModel.extents.Add(extentModel);
                }

                result.Add(workspaceModel);
            }

            return result;
        }
    }
}