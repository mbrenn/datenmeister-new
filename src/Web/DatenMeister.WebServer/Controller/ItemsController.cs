﻿using System;
using System.Linq;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Modules.Json;
using DatenMeister.Modules.PublicSettings;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public ItemsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }
        
        public class DeleteItemParams
        {
            public string Workspace { get; set; } = string.Empty;
            public string ExtentUri { get; set; } = string.Empty;
            public string ItemId { get; set; } = string.Empty;
        }
        
        [HttpPost("api/items/delete")]
        public ActionResult<object> DeleteItem([FromForm] string workspace, [FromForm] string extentUri, [FromForm] string itemId)
        {
            var foundItem = _workspaceLogic.FindItem(workspace, extentUri, itemId);
            if (foundItem != null)
            {
            }

            throw new InvalidOperationException("Deletion is not possible from the subitem");
            // return new {success = true};
        }

        /// <summary>
        /// Deletes an item from the extent itself
        /// </summary>
        /// <param name="param">Parameter of the deletion</param>
        /// <returns>the value indicating the success or not</returns>
        [HttpPost("api/items/delete_from_extent")]
        public ActionResult<object> DeleteFromExtent(
            [FromBody] DeleteItemParams param)
        {
            var extent = _workspaceLogic.FindExtent(param.Workspace, param.ExtentUri)
                         ?? throw new InvalidOperationException("Extent is not found");

            var found = extent.elements().FirstOrDefault(x => (x as IHasId)?.Id == param.ItemId)
                        ?? throw new InvalidOperationException("Item is not found");

            extent.elements().remove(found);
            return new {success = true};
        }

        public class CreateNewObjectForExtentParams
        {
            public string Workspace { get; set; } = string.Empty;
            public string ExtentUri { get; set; } = string.Empty;
            public string? MetaClassUri { get; set; }
        }

        /// <summary>
        /// Creates a new object for the extent and adds it
        /// </summary>
        /// <param name="param">Parameter of the creation</param>
        /// <returns>the action result</returns>
        [HttpPost("api/items/create")]
        public ActionResult<object> CreateNewObjectForExtent([FromBody] CreateNewObjectForExtentParams param)
        {
            var extent = _workspaceLogic.FindExtent(param.Workspace, param.ExtentUri);
            if (extent == null)
            {
                throw new InvalidOperationException("Extent is not found");
            }

            IElement? metaClass = null;
            if (param.MetaClassUri != null)
            {
                metaClass = 
                    _workspaceLogic.GetWorkspace(WorkspaceNames.WorkspaceManagement)
                    ?.FindElementByUri(param.MetaClassUri) as IElement;
            }
            
            var factory = new MofFactory(extent);
            var newElement = factory.create(metaClass);
            extent.elements().add(newElement);
            
            return new {success = true};
        }

        [HttpGet("api/items/get/{workspace}/{extentUri}/{item}")]
        public ActionResult<object> GetItem(string workspace, string extentUri, string item)
        {
            workspace = HttpUtility.UrlDecode(workspace);
            extentUri = HttpUtility.UrlDecode(extentUri);
            item = HttpUtility.UrlDecode(item);

            var extent = _workspaceLogic.FindExtent(workspace, extentUri) as IUriExtent;
            if (extent == null)
            {
                throw new InvalidOperationException("Extent is not found");
            }

            var foundElement = extent.element("#" + item);
            if (foundElement == null)
            {
                throw new InvalidOperationException("Element is not found");
            }

            var converter = new DirectJsonConverter();
            var convertedElement = converter.ConvertToJson(foundElement);

            return new
            {
                item = convertedElement
            };
        }

        [HttpGet("api/items/get/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetItem(string workspaceId, string itemUri)
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

            var converter = new DirectJsonConverter();
            var convertedElement = converter.ConvertToJson(foundElement);

            return new
            {
                item = convertedElement
            };
        }
    }
}