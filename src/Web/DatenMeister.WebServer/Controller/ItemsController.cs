// Configuration parameter which limits the number of elements

#define LimitNumberOfElements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly ItemsControllerInternal _internal;
        private readonly IScopeStorage _scopeStorage;
        private readonly IWorkspaceLogic _workspaceLogic;

        public ItemsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
            _internal = new ItemsControllerInternal(workspaceLogic, scopeStorage);
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
            var values = createParams.properties?.v;
            if (values != null)
            {
                foreach (var propertyParam in values)
                {
                    var value = propertyParam.Value;
                    var propertyValue = DirectJsonDeconverter.ConvertJsonValue(value);

                    if (propertyValue != null)
                    {
                        item.set(propertyParam.Key, propertyValue);
                    }
                }
            }

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

            var item = _internal.GetItemByUriParameter(workspaceId, itemUri);

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

            var values = createParams.properties?.v;
            if (values != null)
                foreach (var propertyParam in values)
                {
                    var value = propertyParam.Value;
                    var propertyValue = DirectJsonDeconverter.ConvertJsonValue(value);

                    if (propertyValue != null) child.set(propertyParam.Key, propertyValue);
                }

            return new
            {
                success = true,
                itemId = (child as IHasId)?.Id ?? string.Empty
            };
        }

        [HttpDelete("api/items/delete/{workspaceId}/{itemId}")]
        public ActionResult<object> DeleteItem(string workspaceId, string itemId)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemId = HttpUtility.UrlDecode(itemId);

            var success = false;
            var foundItem = _workspaceLogic.FindItem(workspaceId, itemId);
            if (foundItem != null)
            {
                success = ObjectHelper.DeleteObject(foundItem);
            }

            return new {success = success};
        }

        /// <summary>
        /// Deletes an item from the extent itself
        /// </summary>
        /// <param name="extentUri">Uri of the extent</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="itemId">Id of the item to be deleted</param>
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
            return new {success = true};
        }

        [HttpGet("api/items/get/{workspaceId}/{extentUri}/{itemId}")]
        public ActionResult<object> GetItem(string workspaceId, string extentUri, string itemId)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);
            itemId = HttpUtility.UrlDecode(itemId);

            var foundElement = _internal.GetItemInternal(workspaceId, extentUri, itemId, out var converter);
            var convertedElement = converter.ConvertToJson(foundElement);

            return convertedElement;
        }

        [HttpGet("api/items/get/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetItem(string workspaceId, string itemUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundElement = _internal.GetItemByUriParameter(workspaceId, itemUri);

            var converter = new MofJsonConverter {MaxRecursionDepth = 2, ResolveReferenceToOtherExtents = true};
            var convertedElement = converter.ConvertToJson(foundElement);

            return convertedElement;
        }

        [HttpGet("api/items/get_root_elements/{workspaceId}/{extentUri}")]
        public ActionResult<object> GetRootElements(string workspaceId, string extentUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);

            var foundElement = _internal.GetItemByUriParameter(workspaceId, extentUri);
            if (foundElement is not IExtent extent)
            {
                return NotFound();
            }

            var converter = new MofJsonConverter {MaxRecursionDepth = 2};

            var result = new StringBuilder();
            result.Append('[');
            var komma = string.Empty;


            var elements = extent.elements().OfType<IElement>();


#if LimitNumberOfElements
#warning Number of elements in ItemsController is limited to improve speed during development. This is not a release option

            elements = elements.Take(100);

#endif

            foreach (var item in elements)
            {
                result.Append(komma);
                result.Append(converter.ConvertToJson(item));

                komma = ", ";
            }

            result.Append(']');

            return result.ToString();
        }

        [HttpPut("api/items/get_container/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetParents(string workspaceId, string itemUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri) as IElement
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

        [HttpPut("api/items/set_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetProperty(string workspaceId, string itemUri,
            [FromBody] SetPropertyParams propertyParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foundItem.set(propertyParams.Key, propertyParams.Value);

            return new {success = true};
        }

        [HttpPost("api/items/unset_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> UnsetProperty(string workspaceId, string itemUri,
            [FromBody] UnsetPropertyParams propertyParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foundItem.unset(propertyParams.Key);

            return new {success = true};
        }

        [HttpPut("api/items/set_properties/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetProperties(string workspaceId, string itemUri,
            [FromBody] SetPropertiesParams propertiesParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foreach (var propertyParam in propertiesParams.Properties)
            {
                foundItem.set(propertyParam.Key, propertyParam.Value);
            }

            return new {success = true};
        }

        [HttpGet("api/items/get_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetProperty(string workspaceId, string itemUri, string property)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var result = _internal.GetPropertyInternal(workspaceId, itemUri, property);

            var converter = new MofJsonConverter {MaxRecursionDepth = 2, ResolveReferenceToOtherExtents = true};
            return Content($"{{\"v\": {converter.ConvertToJson(result)}}}", "application/json", Encoding.UTF8);
        }

        [HttpPost("api/items/set/{workspaceId}/{itemUri}")]
        public ActionResult<object> Set(string workspaceId, string itemUri,
            [FromBody] MofObjectAsJson jsonObject)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foreach (var propertyParam in jsonObject.v)
            {
                var value = propertyParam.Value;
                var propertyValue = DirectJsonDeconverter.ConvertJsonValue(value);

                if (propertyValue != null)
                {
                    foundItem.set(propertyParam.Key, propertyValue);
                }
            }

            return new {success = true};
        }

        [HttpPost("api/items/set_metaclass/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetMetaClass(string workspaceId, string itemUri,
            [FromBody] SetMetaClassParams parameter)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            if (foundItem is IElementSetMetaClass asMetaClassSet)
            {
                asMetaClassSet.SetMetaClass(new MofObjectShadow(parameter.metaClass));
            }

            return new {success = true};
        }

        /// <summary>
        ///     Adds a reference to a property's collection
        /// </summary>
        /// <param name="workspaceId">Id of the workspace where the item can be found</param>
        /// <param name="itemUri">Uri of the item to whose property shall be added</param>
        /// <param name="parameters">Parameters describing the property and the reference</param>
        /// <returns></returns>
        [HttpPost("api/items/add_ref_to_collection/{workspaceId}/{itemUri}")]
        public ActionResult<object> AddReferenceToCollection(string workspaceId, string itemUri,
            [FromBody] AddReferenceToCollectionParams parameters)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");

            var reference = _internal.GetItemByUriParameter(
                                parameters.WorkspaceId,
                                parameters.ReferenceUri)
                            ?? throw new InvalidOperationException("Reference was not found");

            foundItem.AddCollectionItem(parameters.Property, reference);

            return new {success = true};
        }

        /// <summary>
        ///     Adds a reference to a property's collection
        /// </summary>
        /// <param name="workspaceId">Id of the workspace where the item can be found</param>
        /// <param name="itemUri">Uri of the item to whose property shall be added</param>
        /// <param name="parameters">Parameters describing the property and the reference</param>
        /// <returns></returns>
        [HttpPost("api/items/remove_ref_to_collection/{workspaceId}/{itemUri}")]
        public ActionResult<object> RemoveReferenceToCollection(string workspaceId, string itemUri,
            [FromBody] RemoveReferenceToCollectionParams parameters)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");

            var reference = _internal.GetItemByUriParameter(
                                parameters.WorkspaceId,
                                parameters.ReferenceUri)
                            ?? throw new InvalidOperationException("Reference was not found");

            foundItem.RemoveCollectionItem(parameters.Property, reference);

            return new {success = true};
        }

        public class SetMetaClassParams
        {
            public string metaClass { get; set; } = string.Empty;
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

            public MofObjectAsJson? properties { get; set; }
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

            public MofObjectAsJson? properties { get; set; }
        }

        /// <summary>
        ///     Defines the property for the unset property
        /// </summary>
        public class UnsetPropertyParams
        {
            /// <summary>
            ///     Gets or sets the key
            /// </summary>
            public string Key { get; set; } = string.Empty;
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


        /// <summary>
        ///     Defines the parameters to add a reference to a property's collection.
        /// </summary>
        public class AddReferenceToCollectionParams
        {
            /// <summary>
            ///     Defines the property to which the property will be added
            /// </summary>
            public string Property { get; set; } = string.Empty;

            public string? WorkspaceId { get; set; } = null;

            public string ReferenceUri { get; set; } = string.Empty;
        }

        public class RemoveReferenceToCollectionParams
        {
            /// <summary>
            ///     Defines the property to which the property will be removed
            /// </summary>
            public string Property { get; set; } = string.Empty;

            public string? WorkspaceId { get; set; } = null;

            public string ReferenceUri { get; set; } = string.Empty;
        }
    }
}