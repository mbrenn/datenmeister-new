using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;
using DatenMeister.WebServer.Models;
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

        /// <summary>
        /// Parameters to create an item within an extent
        /// </summary>
        public class CreateItemInExtentParams
        {
            /// <summary>
            /// Gets or sets the metaclass
            /// </summary>
            public string? metaClass { get; set; }
        }

        [HttpPost("api/items/create_in_extent/{workspaceId}/{extentUri}")]
        public ActionResult<object> CreateItemInExtent(
            string workspaceId, string extentUri,
            [FromBody] CreateItemInExtentParams createParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);

            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri)
                         ?? throw new InvalidOperationException("Extent is not found");

            var factory = new MofFactory(extent);

            IElement? metaClass =
                string.IsNullOrEmpty(createParams.metaClass)
                    ? null
                    : extent.GetUriResolver().Resolve(createParams.metaClass, ResolveType.OnlyMetaClasses) as IElement;

            var item = factory.create(metaClass);
            extent.elements().add(item);

            return new
            {
                success = true,
                itemId = (item as IHasId)?.Id ?? string.Empty
            };
        }

        /// <summary>
        /// Parameters to create an item within an extent
        /// </summary>
        public class CreateChildParams
        {
            /// <summary>
            /// Gets or sets the metaclass
            /// </summary>
            public string metaClass { get; set; } = string.Empty;

            /// <summary>
            /// Property in which the item shall be added
            /// </summary>
            public string property { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets whether the child shall be added as a list item or
            /// shall be directly set to the property
            /// </summary>
            public bool asList { get; set; }
        }


        [HttpPost("api/items/create_child/{workspaceId}/{itemID}")]
        public ActionResult<object> CreateItemAsChild(
            string workspaceId, string itemId,
            [FromBody] CreateChildParams createParams)
        {
            var item = GetItemByUriParameter(workspaceId, itemId);

            var factory = new MofFactory(item);

            IElement? metaClass =
                string.IsNullOrEmpty(createParams.metaClass)
                    ? null
                    : 
                        (item.GetUriResolver() ?? throw new InvalidOperationException("No UriResolver"))
                        .Resolve(createParams.metaClass, ResolveType.OnlyMetaClasses) as IElement;

            var child = factory.create(metaClass);
            if (createParams.asList)
            {
                item.AddCollectionItem(createParams.property, child);
            }
            else
            {
                item.set(createParams.property, child);
            }

            return new
            {
                success = true,
                itemId = (child as IHasId)?.Id ?? string.Empty
            };
        }

        public class DeleteItemParams
        {
            public string ItemId { get; set; } = string.Empty;
        }
        
        [HttpPost("api/items/delete/{workspaceId}/{extentUri}")]
        public ActionResult<object> DeleteItem(string workspaceId, string extentUri, [FromBody] DeleteItemParams deleteParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);

            var foundItem = _workspaceLogic.FindItem(workspaceId, extentUri, deleteParams.ItemId);
            if (foundItem != null)
            {
            }

            throw new InvalidOperationException("Deletion is not possible from the subitem");
            // return new {success = true};
        }

        /// <summary>
        /// Deletes an item from the extent itself
        /// </summary>
        /// <param name="extentUri">Uri of the extent</param>
        /// <param name="param">Parameter of the deletion</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns>the value indicating the success or not</returns>
        [HttpPost("api/items/delete_from_extent/{workspaceId}/{extentUri}")]
        public ActionResult<object> DeleteFromExtent(
            string workspaceId,
            string extentUri,
            [FromBody] DeleteItemParams param)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri)
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

        [HttpGet("api/items/get/{workspaceId}/{extentUri}/{item}")]
        public ActionResult<object> GetItem(string workspaceId, string extentUri, string item)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);
            item = HttpUtility.UrlDecode(item);

            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri) as IUriExtent;
            if (extent == null)
            {
                throw new InvalidOperationException("Extent is not found");
            }

            var foundElement = extent.element("#" + item);
            if (foundElement == null)
            {
                throw new InvalidOperationException("Element is not found");
            }

            var converter = new MofJsonConverter();
            var convertedElement = converter.ConvertToJson(foundElement);

            return new
            {
                item = convertedElement
            };
        }

        [HttpGet("api/items/get/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetItem(string workspaceId, string itemUri)
        {
            var foundElement = GetItemByUriParameter(workspaceId, itemUri);

            var converter = new MofJsonConverter();
            var convertedElement = converter.ConvertToJson(foundElement);

            var metaClass = (foundElement as IElement)?.getMetaClass();

            return new
            {
                item = convertedElement,
                metaClass = ItemWithNameAndId.Create(metaClass)
            };
        }

        [HttpPut("api/items/get_container/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetParents(string workspaceId, string itemUri)
        {
            var foundItem = GetItemByUriParameter(workspaceId, itemUri) as IElement
                            ?? throw new InvalidOperationException("Item was not found");

            var result = new List<ItemWithNameAndId>();
            var container = foundItem;
            do
            {
                container = container.container();
                result.Add(ItemWithNameAndId.Create(container)
                           ?? throw new InvalidOperationException("Should not happen"));
            } while (container != null);

            return result;
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

        /// <summary>
        /// Defines the property for the set property
        /// </summary>
        public class SetPropertyParams
        {
            /// <summary>
            /// Gets or sets the key
            /// </summary>
            public string Key { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the value
            /// </summary>
            public string Value { get; set; } = string.Empty;
        }

        /// <summary>
        /// Defines the properties for the set properties
        /// </summary>
        public class SetPropertiesParams
        {
            public List<SetPropertyParams> Properties = new();
        }

        [HttpPut("api/items/set_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetProperty (string workspaceId, string itemUri, [FromBody] SetPropertyParams propertyParams)
        {
            var foundItem = GetItemByUriParameter(workspaceId, itemUri);
            foundItem.set(propertyParams.Key, propertyParams.Value);

            return new
            {
                success = true
            };
        }

        [HttpPut("api/items/set_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetProperties(string workspaceId, string itemUri,
            [FromBody] SetPropertiesParams propertiesParams)
        {
            var foundItem = GetItemByUriParameter(workspaceId, itemUri);
            foreach (var propertyParam in propertiesParams.Properties)
            {
                foundItem.set(propertyParam.Key, propertyParam.Value);
            }

            return new {success = true};
        }
    }
}