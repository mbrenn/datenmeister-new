using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;
using DatenMeister.Types;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;
        private readonly LocalTypeSupport _localTypeSupport;

        public TypesController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;

            _localTypeSupport = new LocalTypeSupport(_workspaceLogic, _scopeStorage);
        }

        [HttpGet("api/types/all")]
        public ActionResult<List<ItemWithNameAndId>> GetTypes()
        {
            var result = new List<ItemWithNameAndId>();

            var allTypes = _localTypeSupport.GetAllTypes();
            
            result.AddRange(
                allTypes.OfType<IObject>().Select(o => ItemWithNameAndId.Create(o)!));
            
            return result;
        }
    }
}