using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Json;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ItemsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
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


        [HttpPost("api/items/create_child/{workspaceId}/{itemUri}")]
        public ActionResult<object> CreateItemAsChild(
            string workspaceId, string itemUri,
            [FromBody] CreateChildParams createParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);
            
            var item = GetItemByUriParameter(workspaceId, itemUri);

            var factory = new MofFactory(item);

            IElement? metaClass =
                string.IsNullOrEmpty(createParams.metaClass)
                    ? null
                    : (item.GetUriResolver() ?? throw new InvalidOperationException("No UriResolver"))
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

        [HttpDelete("api/items/delete/{workspaceId}/{itemId}")]
        public ActionResult<object> DeleteItem(string workspaceId,  string itemId)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemId = HttpUtility.UrlDecode(itemId);

            var foundItem = _workspaceLogic.FindItem(workspaceId, itemId);
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
        [HttpPost("api/items/delete_from_extent/{workspaceId}/{extentUri}/{itemId}")]
        public ActionResult<object> DeleteFromExtent(
            string workspaceId,
            string extentUri,
            string itemId)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);
            itemId = HttpUtility.UrlDecode(itemId);
            
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri)
                         ?? throw new InvalidOperationException("Extent is not found");

            var found = extent.elements().FirstOrDefault(x => (x as IHasId)?.Id == itemId)
                        ?? throw new InvalidOperationException("Item is not found");

            extent.elements().remove(found);
            return new { success = true };
        }

        [HttpGet("api/items/get/{workspaceId}/{extentUri}/{itemId}")]
        public ActionResult<object> GetItem(string workspaceId, string extentUri, string itemId)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);
            itemId = HttpUtility.UrlDecode(itemId);

            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri) as IUriExtent;
            if (extent == null)
            {
                throw new InvalidOperationException("Extent is not found");
            }

            var foundElement = extent.element("#" + itemId);
            if (foundElement == null)
            {
                throw new InvalidOperationException("Element is not found");
            }

            var converter = new MofJsonConverter() { MaxRecursionDepth = 2 };
            var convertedElement = converter.ConvertToJson(foundElement);

            return convertedElement;
        }

        [HttpGet("api/items/get/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetItem(string workspaceId, string itemUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);
            
            var foundElement = GetItemByUriParameter(workspaceId, itemUri);

            var converter = new MofJsonConverter() { MaxRecursionDepth = 2 };
            var convertedElement = converter.ConvertToJson(foundElement);

            var metaClass = (foundElement as IElement)?.getMetaClass();

            return convertedElement;
        }

        [HttpGet("api/items/get_root_elements/{workspaceId}/{extentUri}")]
        public ActionResult<object> GetRootElements(string workspaceId, string extentUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);
            
            var foundElement = GetItemByUriParameter(workspaceId, extentUri);
            if (foundElement is not IExtent extent)
            {
                return NotFound();
            }

            var converter = new MofJsonConverter { MaxRecursionDepth = 2 };

            var result = new StringBuilder();
            result.Append("[");
            var komma = string.Empty;

            foreach (var item in extent.elements().OfType<IElement>())
            {
                result.Append(komma);
                result.Append(converter.ConvertToJson(item));

                komma = ", ";
            }

            result.Append("]");

            return result.ToString();
        }

        [HttpPut("api/items/get_container/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetParents(string workspaceId, string itemUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);
            
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
            var workspace = _workspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                throw new InvalidOperationException("Extent is not found");
            }

            if (workspace.Resolve(itemUri, ResolveType.NoMetaWorkspaces) is not IObject foundElement)
            {
                throw new InvalidOperationException("Element is not found");
            }

            return foundElement;
        }

        [HttpPut("api/items/set_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetProperty(string workspaceId, string itemUri,
            [FromBody] SetPropertyParams propertyParams)
        {            
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);
            
            var foundItem = GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foundItem.set(propertyParams.Key, propertyParams.Value);

            return new { success = true };
        }

        [HttpPut("api/items/set_properties/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetProperties(string workspaceId, string itemUri,
            [FromBody] SetPropertiesParams propertiesParams)
        {  
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foreach (var propertyParam in propertiesParams.Properties)
            {
                foundItem.set(propertyParam.Key, propertyParam.Value);
            }

            return new { success = true };
        }

        [HttpPost("api/items/set/{workspaceId}/{itemUri}")]
        public ActionResult<object> Set(string workspaceId, string itemUri,
            [FromBody] MofObjectAsJson jsonObject)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foreach (var propertyParam in jsonObject.v)
            {
                var value = propertyParam.Value;
                object? propertyValue = null;
                if (value is JsonElement jsonElement)
                {
                    propertyValue = jsonElement.ValueKind switch
                    {
                        JsonValueKind.String => jsonElement.GetString(),
                        JsonValueKind.Number => jsonElement.GetDouble(),
                        JsonValueKind.True => true,
                        JsonValueKind.False => false,
                        _ => propertyValue
                    };
                }

                if (propertyValue != null)
                {
                    foundItem.set(propertyParam.Key, propertyValue);
                }
            }

            return new { success = true };
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
    }
}