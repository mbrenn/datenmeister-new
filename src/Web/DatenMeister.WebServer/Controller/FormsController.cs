﻿using System;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Forms;
using DatenMeister.Modules.Json;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public FormsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }
        
        [HttpGet("api/forms/default_for_item/{workspaceId}/{itemUrl}/{viewMode?}")]
        public ActionResult<IItem> GetDefaultFormForItem(string workspaceId, string itemUrl, string viewMode)
        {
            viewMode = HttpUtility.UrlDecode(viewMode);

            var item = GetItemByUriParameter(workspaceId, itemUrl);
                
            var formLogic = new FormsPlugin(_workspaceLogic, _scopeStorage);
            var form = formLogic.GetItemTreeFormForObject(item, FormDefinitionMode.Default, viewMode);
            if (form == null)
            {
                throw new InvalidOperationException("Form is not defined");
            }

            return new IItem
            {
                item = DirectJsonConverter.ConvertToJsonWithDefaultParameter(form),
                metaClass = ItemWithNameAndId.Create(form.getMetaClass())
            };
        }
        
        [HttpGet("api/forms/default_for_extent/{workspaceId}/{extentUrl}/{viewMode?}")]
        public ActionResult<IItem> GetDefaultFormForExtent(string workspaceId, string extentUrl, string viewMode)
        {
            viewMode = HttpUtility.UrlDecode(viewMode);
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUrl = HttpUtility.UrlDecode(extentUrl);

            var extent = _workspaceLogic.FindExtent(workspaceId, extentUrl)
                         ?? throw new InvalidOperationException("Extent is not found");
                
            var formLogic = new FormsPlugin(_workspaceLogic, _scopeStorage);
            var form = formLogic.GetExtentForm(extent, FormDefinitionMode.Default, viewMode);
            if (form == null)
            {
                throw new InvalidOperationException("Form is not defined");
            }

            return new IItem
            {
                item = DirectJsonConverter.ConvertToJsonWithDefaultParameter(form),
                metaClass = ItemWithNameAndId.Create(form.getMetaClass())
            };
        }

        /// <summary>
        /// Gets the items by the uri parameter.
        /// The parameter themselves are expected to be uriencoded, so a decoding via HttpUtility.UrlDecode will be performed
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="itemUri">Uri of the item</param>
        /// <returns>The found object</returns>
        private IObject GetItemByUriParameter(string workspaceId, string itemUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var workspace = _workspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                throw new InvalidOperationException("Extent is not found");
            }

            var foundElement = workspace.Resolve(itemUri, ResolveType.NoMetaWorkspaces) as IObject;
            if (foundElement == null)
            {
                throw new InvalidOperationException("Element is not found");
            }

            return foundElement;
        }
    }
}