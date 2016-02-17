using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Web.Models.PostModels;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/workspace")]
    public class WorkspaceController : ApiController
    {
        private readonly IWorkspaceCollection _workspaceCollection;

        public WorkspaceController(IWorkspaceCollection workspaceCollection)
        {
            _workspaceCollection = workspaceCollection;
        }

        [Route("all")]
        public object Get()
        {
            var result = new List<object>();
            foreach (var workspace in _workspaceCollection.Workspaces)
            {
                result.Add(
                    new
                    {
                        workspace.id,
                        workspace.annotation
                    });
            }

            return result;
        }

        [Route("create")]
        [HttpPost]
        public object Create([FromBody] WorkspaceCreateModel model)
        {
            var workspace = new Workspace<IExtent>(model.name, model.annotation);
            _workspaceCollection.AddWorkspace(workspace);

            return new {success = true};
        }

        [Route("delete")]
        [HttpPost]
        public object Delete([FromBody] WorkspaceDeleteModel model)
        {
            _workspaceCollection.RemoveWorkspace(model.name);

            return new { success = true };
        }
    }
}