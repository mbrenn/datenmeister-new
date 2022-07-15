using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly FormsControllerInternal _internal;

        public FormsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _internal = new FormsControllerInternal(workspaceLogic, scopeStorage);
        }

        [HttpGet("api/forms/get/{formUri}")]
        public ActionResult<string> Get(string formUri)
        {
            formUri = HttpUtility.UrlDecode(formUri);
            var form = _internal.GetInternal(formUri);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/default_for_item/{workspaceId}/{itemUrl}/{viewMode?}")]
        public ActionResult<string> GetObjectFormForItem(string workspaceId, string itemUrl, string? viewMode)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUrl = HttpUtility.UrlDecode(itemUrl);
            viewMode = HttpUtility.UrlDecode(viewMode);

            var form = _internal.GetObjectFormForItemInternal(workspaceId, itemUrl, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/default_for_extent/{workspaceId}/{extentUri}/{viewMode?}")]
        public ActionResult<string> GetCollectionFormForExtent(string workspaceId, string extentUri, string? viewMode)
        {
            viewMode = HttpUtility.UrlDecode(viewMode);
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);

            var form = _internal.GetCollectionFormForExtentInternal(workspaceId, extentUri, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/default_object_for_metaclass/{metaClass}/{viewMode?}")]
        public ActionResult<string> GetObjectFormForMetaClass(string? metaClass, string? viewMode) 
        {
            viewMode = HttpUtility.UrlDecode(viewMode);
            metaClass = HttpUtility.UrlDecode(metaClass);

            var form = _internal.GetObjectFormForMetaClassInternal(metaClass, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/get_viewmodes")]
        public ActionResult<GetViewModesResult> GetViewModes()
        {
            var viewModes = _internal.GetViewModesInternal();
            return new GetViewModesResult
            {
                ViewModes = viewModes
                    .Select(x=> MofJsonConverter.ConvertToJsonWithDefaultParameter(x))
                    .ToList()
            };
        }

        public record GetViewModesResult
        {
            /// <summary>
            /// A list of view modes
            /// </summary>
            public List<string> ViewModes { get; set; } = new();
        }
    }
}