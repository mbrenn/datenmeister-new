using System;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Json;
using DatenMeister.TemporaryExtent;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class ElementsController : ControllerBase
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public readonly ElementsControllerInternal Internal;

        public ElementsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            Internal = new ElementsControllerInternal(workspaceLogic, scopeStorage);
        }

        [HttpGet("api/elements/get_name/{workspace}/{extentUri}/{itemId}")]
        public ActionResult<object> GetName(string workspace, string extentUri, string itemId)
        {
            var foundItem = _workspaceLogic.FindObject(workspace, extentUri, itemId);
            if (foundItem == null)
            {
                return NotFound();
            }

            return ItemWithNameAndId.Create(foundItem)!;
        }

        [HttpGet("api/elements/get_name/{workspace}/{uri}")]
        public ActionResult<ItemWithNameAndId> GetName(string? workspace, string uri)
        {
            IElement? foundItem;
            if (string.IsNullOrEmpty(workspace) || workspace == "_")
            {
                foundItem = _workspaceLogic.FindItem(HttpUtility.UrlDecode(uri));
            }
            else
            {
                foundItem =
                    _workspaceLogic.GetWorkspace(workspace)?.Resolve(HttpUtility.UrlDecode(uri), ResolveType.NoMetaWorkspaces)
                        as IElement;
            }

            if (foundItem == null)
            {
                return NotFound();
            }

            return ItemWithNameAndId.Create(foundItem)!;
        }

        [HttpGet("api/elements/get_composites/{workspaceId?}/{itemUrl?}")]
        public ActionResult<ItemWithNameAndId[]> GetComposites(string? workspaceId, string? itemUrl)
        {
            var result = Internal.GetComposites(workspaceId, itemUrl);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpGet("api/elements/find_by_searchstring")]
        public ActionResult<FindBySearchStringResult> FindBySearchString(string search)
        {
            return Internal.FindBySearchString(search);
        }

        public class FindBySearchStringResult
        {
            public const string ResultTypeNone = "none";
            public const string ResultTypeReference = "reference";
            public const string ResultTypeReferenceExtent = "referenceExtent";
            
            public string resultType { get; set; } = ResultTypeNone;
            
            public ItemWithNameAndId? reference { get; set; }
        }

        [HttpPut("api/elements/create_temporary_element")]
        public ActionResult<CreateTemporaryElementResult> CreateTemporaryElement()
        {
            var logic = new TemporaryExtentLogic(_workspaceLogic);
            var result = logic.CreateTemporaryElement(null);
            return new CreateTemporaryElementResult
            {
                Success = true,
                Uri = result.GetUri() ?? throw new InvalidOperationException("No uri defined")
            };
        }


        /// <summary>
        /// Defines the result of the CreateTemporaryElement method 
        /// </summary>
        public class CreateTemporaryElementResult
        {
            /// <summary>
            /// Gets or sets a flag indicating the success
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// Gets or sets the uri 
            /// </summary>
            public string Uri { get; set; } = string.Empty;
        }
    }
}