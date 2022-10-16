// Configuration parameter which limits the number of elements

#if DEBUG
#define LimitNumberOfElements
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView;
using DatenMeister.Json;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.WebServer.Models;
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

            var metaClass =
                string.IsNullOrEmpty(createParams.metaClass)
                    ? null
                    : extent.GetUriResolver().Resolve(createParams.metaClass, ResolveType.OnlyMetaClasses) as IElement;

            var item = factory.create(metaClass);
            var values = createParams.properties?.v;
            if (values != null)
                foreach (var propertyParam in values)
                {
                    var value = propertyParam.Value;
                    var propertyValue = new DirectJsonDeconverter(_workspaceLogic).ConvertJsonValue(value);

                    if (propertyValue != null) item.set(propertyParam.Key, propertyValue);
                }

            extent.elements().add(item);

            return new CreateItemInExtentResult {success = true, itemId = (item as IHasId)?.Id ?? string.Empty};
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

        public class CreateItemInExtentResult
        {
            public bool success { get; set; }
            public string itemId { get; set; } = string.Empty;
        }

        [HttpPost("api/items/create_child/{workspaceId}/{itemUri}")]
        public ActionResult<object> CreateItemAsChild(
            string workspaceId, string itemUri,
            [FromBody] CreateItemAsChildParams createItemAsParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var item = _internal.GetItemByUriParameter(workspaceId, itemUri);

            var factory = new MofFactory(item);

            var metaClass =
                string.IsNullOrEmpty(createItemAsParams.metaClass)
                    ? null
                    : (item.GetUriResolver() ?? throw new InvalidOperationException("No UriResolver"))
                    .Resolve(createItemAsParams.metaClass, ResolveType.OnlyMetaClasses) as IElement;

            var child = factory.create(metaClass);
            if (createItemAsParams.asList)
                item.AddCollectionItem(createItemAsParams.property, child);
            else
                item.set(createItemAsParams.property, child);

            var values = createItemAsParams.properties?.v;
            if (values != null)
                foreach (var propertyParam in values)
                {
                    var value = propertyParam.Value;
                    var propertyValue = new DirectJsonDeconverter(_workspaceLogic).ConvertJsonValue(value);

                    if (propertyValue != null) child.set(propertyParam.Key, propertyValue);
                }

            return new CreateItemAsChildResult {success = true, itemId = (child as IHasId)?.Id ?? string.Empty};
        }

        /// <summary>
        /// Parameters to create an item within an extent
        /// </summary>
        public class CreateItemAsChildParams
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


        public class CreateItemAsChildResult
        {
            public bool success { get; set; }
            public string itemId { get; set; } = string.Empty;
        }

        [HttpDelete("api/items/delete/{workspaceId}/{itemUrl}")]
        public ActionResult<object> DeleteItem(string workspaceId, string itemUrl)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUrl = HttpUtility.UrlDecode(itemUrl);

            var success = false;
            var foundItem = _workspaceLogic.FindItem(workspaceId, itemUrl);
            if (foundItem != null) success = ObjectHelper.DeleteObject(foundItem);

            return new {success = success};
        }

        /// <summary>
        /// Deletes all elements from an extent
        /// </summary>
        /// <param name="workspaceId">Id of the workspace in which the extent is residing</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <returns>The action result</returns>
        [HttpDelete("api/items/delete_root_elements/{workspaceId}/{extentUri}")]
        public ActionResult<SuccessResult> DeleteRootElements(string workspaceId, string extentUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);

            var success = true;
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri)
                         ?? throw new InvalidOperationException("Extent is not found");
            
            extent.elements().RemoveAll();

            return new SuccessResult {Success = success};
        }

        /// <summary>
        /// Deletes an item from the extent itself
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <param name="itemId">Id of the item to be deleted</param>
        /// <returns>the value indicating the success or not</returns>
        [HttpPost("api/items/delete_from_extent/{workspaceId}/{itemUri}")]
        public ActionResult<object> DeleteItemFromExtent(
            string workspaceId,
            string itemUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundElement = _internal.GetItemByUriParameter(workspaceId, itemUri);
            var extent = foundElement.GetExtentOf();
            if (extent == null)
            {
                throw new InvalidOperationException($"Extent of item {itemUri} was not found");
            }

            extent.elements().remove(foundElement);
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

        [HttpGet("api/items/get_itemwithnameandid/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetItemWithNameAndId(string workspaceId, string itemUri)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundElement = _internal.GetItemByUriParameter(workspaceId, itemUri);

            return ItemWithNameAndId.Create(foundElement)!;
        }

        /// <summary>
        /// Gets the root elements of a certain extent and workspace
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUri">Uri of the extent from which the items are retrieved</param>
        /// <param name="viewNode">The view node being used to filter the items</param>
        /// <returns></returns>
        [HttpGet("api/items/get_root_elements/{workspaceId}/{extentUri}")]
        public ActionResult<object> GetRootElements(string workspaceId, string extentUri, string? viewNode = null)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            extentUri = HttpUtility.UrlDecode(extentUri);
            viewNode = HttpUtility.UrlDecode(viewNode);

            var foundElement = _internal.GetItemByUriParameter(workspaceId, extentUri);
            if (foundElement is not IExtent extent) return NotFound();

            var converter = new MofJsonConverter {MaxRecursionDepth = 2};

            var result = new StringBuilder();
            result.Append('[');
            var komma = string.Empty;
            
            var elements = extent.elements() as IReflectiveCollection;
            
            /*
             * Checks, if a view node was specified, if a view node was specified, the elements will be filtered
             * according the viewnode
             */
            if (viewNode != null)
            {
                var dataviewHandler =
                    new DataViewEvaluation(_workspaceLogic, _scopeStorage);
                dataviewHandler.AddDynamicSource("input", elements);

                var viewNodeElement = _workspaceLogic.FindElement(
                    WorkspaceNames.WorkspaceManagement, viewNode);
                if (viewNodeElement != null)
                {
                    elements = dataviewHandler.GetElementsForViewNode(viewNodeElement);
                }
                else
                {
                    return new NotFoundResult();
                }
            }

#if LimitNumberOfElements
#warning Number of elements in ItemsController is limited to improve speed during development. This is not a release option
            var finalElements = elements.Take(100).ToList();
#else
            var finalElements = elements.ToList()
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

        [HttpGet("api/items/get_container/{workspaceId}/{itemUri}")]
        public ActionResult<List<ItemWithNameAndId>> GetContainer(string workspaceId, string itemUri, bool? self = false)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);
            var result = new List<ItemWithNameAndId>();

            IUriExtent? extent;
            
            // Checks, if we have found the item
            if (_internal.GetItemByUriParameter(workspaceId, itemUri) is IElement foundItem)
            {
                // Yes, the item was found
                var container = foundItem;

                if (self != true)
                {
                    // If the element itself shall not be returned...
                    container = container.container();
                }

                var maxIteration = 100;
                do
                {
                    if (container == null)
                    {
                        // Container is null
                        break;
                    }

                    result.Add(ItemWithNameAndId.Create(container)
                               ?? throw new InvalidOperationException("Should not happen"));
                    maxIteration--;

                    container = container.container(); // Now go one container up
                } while (maxIteration > 0);
                
                extent = foundItem.GetExtentOf() as IUriExtent;
            }
            else
            {
                extent = _workspaceLogic.FindExtent(workspaceId, itemUri) as IUriExtent;
            }

            // After we are at the root element, now return the management items for extent and workspace
            // First the extent
            var workspace = extent?.GetWorkspace();
            if (extent != null && workspace != null)
            {
                result.Add(ItemWithNameAndId.Create(extent, EntentType.Extent)
                           ?? throw new InvalidOperationException("Should not happen"));

                var managementWorkspaceItem = _workspaceLogic.GetManagementWorkspace()
                    .ResolveById(ExtentManagementUrlHelper.GetIdOfWorkspace(workspace.id));
                if (managementWorkspaceItem != null)
                {
                    result.Add(ItemWithNameAndId.Create(managementWorkspaceItem, EntentType.Workspace)
                               ?? throw new InvalidOperationException("Should not happen"));
                }
            }

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

        [HttpPost("api/items/set_property_reference/{workspaceId}/{itemUri}")]
        public ActionResult<SuccessResult> SetPropertyReference(string workspaceId, string itemUri,
            [FromBody] SetPropertyReferenceParams parameters)
        {

            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");

            var reference = _internal.GetItemByUriParameter(
                                parameters.WorkspaceId,
                                parameters.ReferenceUri)
                            ?? throw new InvalidOperationException("Reference was not found");

            foundItem.set(parameters.Property, reference);

            return new SuccessResult() {Success = true};
        }

        /// <summary>
        /// Defines the property for the set property
        /// </summary>
        public class SetPropertyReferenceParams
        {
            /// <summary>
            /// Gets or sets the key
            /// </summary>
            public string Property { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the value
            /// </summary>
            public string ReferenceUri { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the value of the workspace from which the element will be loaded
            /// </summary>
            public string WorkspaceId { get; set; } = string.Empty;
        }

        [HttpPost("api/items/unset_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> UnsetProperty(string workspaceId, string itemUri,
            [FromBody] UnsetPropertyParams propertyParams)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foundItem.unset(propertyParams.Property);

            return new {success = true};
        }

        /// <summary>
        ///     Defines the property for the unset property
        /// </summary>
        public class UnsetPropertyParams
        {
            /// <summary>
            ///     Gets or sets the key
            /// </summary>
            public string Property { get; set; } = string.Empty;
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

        [HttpGet("api/items/get_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetProperty(string workspaceId, string itemUri, string property)
        {
            workspaceId = HttpUtility.UrlDecode(workspaceId);
            itemUri = HttpUtility.UrlDecode(itemUri);

            var result = _internal.GetPropertyInternal(workspaceId, itemUri, property);

            var converter = new MofJsonConverter {MaxRecursionDepth = 2, ResolveReferenceToOtherExtents = true};
            return Content($"{{\"v\": {converter.ConvertToJson(result)}}}", "application/json", Encoding.UTF8);
        }

        [HttpPut("api/items/set/{workspaceId}/{itemUri}")]
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
                var propertyValue = new DirectJsonDeconverter(_workspaceLogic).ConvertJsonValue(value);

                if (propertyValue != null) foundItem.set(propertyParam.Key, propertyValue);
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
                asMetaClassSet.SetMetaClass(new MofObjectShadow(parameter.metaClass));

            return new {success = true};
        }
        

        public class SetMetaClassParams
        {
            public string metaClass { get; set; } = string.Empty;
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
        ///     Defines the parameters to add a reference to a property's collection.
        /// </summary>
        public class AddReferenceToCollectionParams
        {
            /// <summary>
            ///     Defines the property to which the property will be added
            /// </summary>
            public string Property { get; set; } = string.Empty;

            /// <summary>
            /// Defines the workspace from which the property will be laoded
            /// </summary>
            public string? WorkspaceId { get; set; } = null;

            /// <summary>
            /// Defines the reference Uri from which the property will be loaded
            /// </summary>
            public string ReferenceUri { get; set; } = string.Empty;
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
                            ?? throw new InvalidOperationException(
                                $"Item was not found: {workspaceId}:{itemUri}");

            var reference = _internal.GetItemByUriParameter(
                                parameters.WorkspaceId,
                                parameters.ReferenceUri)
                            ?? throw new InvalidOperationException("Reference was not found");

            foundItem.RemoveCollectionItem(parameters.Property, reference);

            return new {success = true};
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