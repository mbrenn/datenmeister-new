﻿using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<string> GetDefaultFormForItem(string workspaceId, string itemUrl, string? viewMode)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUrl = HttpUtility.UrlDecode(itemUrl);
            viewMode = HttpUtility.UrlDecode(viewMode);

            var form = _internal.GetDefaultFormForItemInternal(workspaceId, itemUrl, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/default_for_extent/{workspaceId}/{extentUri}/{viewMode?}")]
        public ActionResult<string> GetDefaultFormForExtent(string workspaceId, string extentUri, string? viewMode)
        {
            viewMode = HttpUtility.UrlDecode(viewMode);
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);

            var form = _internal.GetDefaultFormForExtentInternal(workspaceId, extentUri, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/default_for_metaclass/{metaClass}/{viewMode?}")]
        public ActionResult<string> GetDefaultFormForMetaClass(string metaClass, string? viewMode) 
        {
            viewMode = HttpUtility.UrlDecode(viewMode);
            metaClass = HttpUtility.UrlDecode(metaClass);

            var form = _internal.GetDefaultFormForMetaClassInternal(metaClass, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }
    }
}