// Configuration parameter which limits the number of elements

#if DEBUG
#define LimitNumberOfElements
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView;
using DatenMeister.Json;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.WebServer.Library.Helper;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

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
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);

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
                    var propertyValue = new DirectJsonDeconverter(_workspaceLogic, _scopeStorage)
                        .ConvertJsonValue(value);

                    if (propertyValue != null) item.set(propertyParam.Key, propertyValue);
                }

            extent.elements().add(item);

            return new CreateItemInExtentResult
            {
                success = true,
                itemId = (item as IHasId)?.Id ?? string.Empty,
                workspace = workspaceId,
                itemUrl = item.GetUri() ?? string.Empty
            };
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

            /// <summary>
            /// Gets or sets the item url
            /// </summary>
            public string itemUrl { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the workspace
            /// </summary>
            public string workspace { get; set; } = string.Empty;
        }

        [HttpPost("api/items/create_child/{workspaceId}/{itemUri}")]
        public ActionResult<object> CreateItemAsChild(
            string workspaceId, string itemUri,
            [FromBody] CreateItemAsChildParams createItemAsParams)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

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
                    var propertyValue = new DirectJsonDeconverter(_workspaceLogic, _scopeStorage)
                        .ConvertJsonValue(value);

                    if (propertyValue != null) child.set(propertyParam.Key, propertyValue);
                }

            return new CreateItemAsChildResult
            {
                success = true,
                itemId = (child as IHasId)?.Id ?? string.Empty,
                workspace = workspaceId,
                itemUrl = child.GetUri() ?? string.Empty
            };
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

            /// <summary>
            /// Gets or sets the item url
            /// </summary>
            public string itemUrl { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the workspace
            /// </summary>
            public string workspace { get; set; } = string.Empty;
        }

        [HttpDelete("api/items/delete/{workspaceId}/{itemUrl}")]
        public ActionResult<object> DeleteItem(string workspaceId, string itemUrl)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUrl = MvcUrlEncoder.DecodePathOrEmpty(itemUrl);

            var success = false;
            var foundItem = _workspaceLogic.FindObject(workspaceId, itemUrl);
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
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);

            var success = true;
            var extent = _workspaceLogic.FindExtent(workspaceId, extentUri);
            if (extent == null)
            {
                return NotFound("Extent is not found");
            }
            
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
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            var foundElement = _internal.GetItemByUriParameter(workspaceId, itemUri);
            var extent = foundElement.GetExtentOf();
            if (extent == null)
            {
                return NotFound($"Extent of item {itemUri} was not found");
            }

            extent.elements().remove(foundElement);
            return new {success = true};
        }

        [HttpGet("api/items/get/{workspaceId}/{extentUri}/{itemId}")]
        public ActionResult<object> GetItem(string workspaceId, string extentUri, string itemId)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);
            itemId = MvcUrlEncoder.DecodePathOrEmpty(itemId);

            var foundElement = _internal.GetItemInternal(workspaceId, extentUri, itemId, out var converter);
            var convertedElement = converter.ConvertToJson(foundElement);

            return convertedElement;
        }

        [HttpGet("api/items/get/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetItem(string workspaceId, string itemUri)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            var foundElement = _internal.GetItemByUriParameter(workspaceId, itemUri);

            var converter = new MofJsonConverter {MaxRecursionDepth = 2, ResolveReferenceToOtherExtents = true};
            var convertedElement = converter.ConvertToJson(foundElement);

            return convertedElement;
        }

        [HttpGet("api/items/get_itemwithnameandid/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetItemWithNameAndId(string workspaceId, string itemUri)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

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
        public ActionResult<string> GetRootElements(string workspaceId, string extentUri, string? viewNode = null)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);
            viewNode = MvcUrlEncoder.DecodePath(viewNode);

            var finalElements = GetRootElementsInternal(workspaceId, extentUri, viewNode);
            if (finalElements == null)
            {
                return NotFound();
            }

            return ConvertToJson(finalElements);
        }

        /// <summary>
        /// Converts the given list of elements to a json array
        /// </summary>
        /// <param name="finalElements">Final elements to be converted</param>
        /// <returns>The json as string</returns>
        private static ActionResult<string> ConvertToJson(IEnumerable<object?> finalElements)
        {
            var converter = new MofJsonConverter { MaxRecursionDepth = 2 };

            var result = new StringBuilder();
            result.Append('[');
            var komma = string.Empty;

            foreach (var item in finalElements)
            {
                result.Append(komma);
                result.Append(converter.ConvertToJson(item));

                komma = ", ";
            }

            result.Append(']');

            return result.ToString();
        }

        /// <summary>
        /// Gets the elements as items as returned by the query
        /// </summary>
        /// <param name="queryUri">Uri to be queried</param>
        /// <returns>Enumeration of items being queried</returns>
        [HttpGet("api/items/get_elements/{queryUri}")]
        public ActionResult<string> GetElements(string queryUri)
        {
            queryUri = MvcUrlEncoder.DecodePathOrEmpty(queryUri);
            var result = _workspaceLogic.Resolve(queryUri, ResolveType.Default, true);
            if (result is IUriExtent asUriExtent)
            {
                result = asUriExtent.elements();
            }

            if (result is IReflectiveCollection reflectiveCollection)
            {
                return ConvertToJson(reflectiveCollection);
            }

            return NotFound($"{queryUri} did not return a reflective collection");
        }

        /// <summary>
        /// Gets the root elements of a certain extent and workspace
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUri">Uri of the extent from which the items are retrieved</param>
        /// <param name="viewNode">The view node being used to filter the items</param>
        /// <returns></returns>
        [HttpGet("api/items/get_root_elements_as_item/{workspaceId}/{extentUri}")]
        public ActionResult<IEnumerable<ItemWithNameAndId>> GetRootElementsAsItem(string workspaceId, string extentUri, string? viewNode = null)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);
            viewNode = MvcUrlEncoder.DecodePath(viewNode);

            var finalElements = GetRootElementsInternal(workspaceId, extentUri, viewNode);
            if (finalElements == null)
            {
                return NotFound($"{extentUri} did not return a reflective collection");
            }

            return finalElements.OfType<IObject>().Select(x => ItemWithNameAndId.Create(x)!).ToList();
        }

        /// <summary>
        /// Gets the elements as items as returned by the query
        /// </summary>
        /// <param name="queryUri">Uri to be queried</param>
        /// <returns>Enumeration of items being queried</returns>
        [HttpGet("api/items/get_elements_as_item/{queryUri}")]
        public ActionResult<IEnumerable<ItemWithNameAndId>> GetElementsAsItem(string queryUri)
        {
            queryUri = MvcUrlEncoder.DecodePathOrEmpty(queryUri);
            var result = _workspaceLogic.Resolve(queryUri, ResolveType.Default, true);
            if (result is IUriExtent asUriExtent)
            {
                result = asUriExtent.elements();
            }

            if (result is IReflectiveCollection reflectiveCollection)
            {
                return reflectiveCollection.OfType<IObject>().Select(x => ItemWithNameAndId.Create(x)!).ToList();
            }

            return NotFound($"{queryUri} did not return a reflective collection");
        }

        private List<object?>? GetRootElementsInternal(string workspaceId, string extentUri, string? viewNode = null)
        {
            var (collection, extent) = _workspaceLogic.FindExtentAndCollection(workspaceId, extentUri);
            if (collection == null || extent == null)
            {
                return null;
            }

            /*
             * Checks, if a view node was specified, if a view node was specified, the elements will be filtered
             * according the viewnode
             */
            if (viewNode != null)
            {
                var dataviewHandler =
                    new DataViewEvaluation(_workspaceLogic, _scopeStorage);
                dataviewHandler.AddDynamicSource("input", collection);

                var viewNodeElement =
                    _workspaceLogic.FindElement(WorkspaceNames.WorkspaceManagement, viewNode)
                    ?? _workspaceLogic.FindElement(WorkspaceNames.WorkspaceData, viewNode);

                if (viewNodeElement != null)
                {
                    collection = dataviewHandler.GetElementsForViewNode(viewNodeElement);
                }
                else
                {
                    return null;
                }
            }

#if LimitNumberOfElements
#warning Number of elements in ItemsController is limited to improve speed during development. This is not a release option
            var finalElements = collection.Take(100).ToList();
#else
            var finalElements = collection.ToList();
#endif

            return finalElements;

        }

        [HttpGet("api/items/get_container/{workspaceId}/{itemUri}")]
        public ActionResult<List<ItemWithNameAndId>> GetContainer(string workspaceId, string itemUri, bool? self = false)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);
            var result = new List<ItemWithNameAndId>();

            IUriExtent? extent;
            
            // Checks, if we have found the item
            var foundItem = _workspaceLogic.FindObjectOrCollection(workspaceId, itemUri);
            
            switch (foundItem)
            {
                case IElement element:
                {
                    // Yes, the item was found
                    var container = element;

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
                
                    extent = element.GetExtentOf() as IUriExtent;
                    break;
                }
                case IUriExtent asExtent:
                    extent = asExtent;
                    break;
                case IReflectiveCollection reflectiveCollection:
                    extent = reflectiveCollection.GetUriExtentOf() as IUriExtent
                             ?? throw new NotImplementedException("Is not a Uri Extent");
                    break;
                default:
                    return NotFound("Found element is neither Element nor ReflectiveCollection nor Extent: " + itemUri);
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
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                            ?? throw new InvalidOperationException("Item was not found");
            foundItem.set(propertyParams.Key, propertyParams.Value);
            return new {success = true};
        }

        [HttpPost("api/items/set_property_reference/{workspaceId}/{itemUri}")]
        public ActionResult<SuccessResult> SetPropertyReference(string workspaceId, string itemUri,
            [FromBody] SetPropertyReferenceParams parameters)
        {

            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri);
            if (foundItem == null)
            {
                return NotFound("Item was not found");
            }


            var reference = _internal.GetItemByUriParameter(
                                parameters.WorkspaceId,
                                parameters.ReferenceUri);
            if (reference == null)
            {
                return NotFound("Reference was not found");
            }

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
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

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

        [HttpPost("api/items/set_properties/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetProperties(string workspaceId, string itemUri,
            [FromBody] SetPropertiesParams propertiesParams)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

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
            public List<SetPropertyParams> Properties { get; set; } = new();
        }

        [HttpGet("api/items/get_property/{workspaceId}/{itemUri}")]
        public ActionResult<object> GetProperty(string workspaceId, string itemUri, string property)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            var result = _internal.GetPropertyInternal(workspaceId, itemUri, property);

            var converter = new MofJsonConverter {MaxRecursionDepth = 2, ResolveReferenceToOtherExtents = true};
            return Content($"{{\"v\": {converter.ConvertToJson(result)}}}", "application/json", Encoding.UTF8);
        }

        [HttpPut("api/items/set/{workspaceId}/{itemUri}")]
        public ActionResult<SuccessResult> Set(string workspaceId, string itemUri,
            [FromBody] MofObjectAsJson jsonObject)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            var converter = new MofJsonDeconverter(_workspaceLogic, _scopeStorage);
            var objectToBeSet = converter.ConvertToObject(jsonObject);
            if (objectToBeSet == null)
            {
                return NotFound("Should not be null");
            }

            var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri);
            if (foundItem == null)
            {
                return NotFound("Item was not found");
            }

            ObjectCopier.CopyPropertiesStatic(objectToBeSet, foundItem);

            return new SuccessResult{Success = true};
        }

        [HttpPost("api/items/set_metaclass/{workspaceId}/{itemUri}")]
        public ActionResult<object> SetMetaClass(string workspaceId, string itemUri,
            [FromBody] SetMetaClassParams parameter)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

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
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

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
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

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
        
        
        public class ExportXmiResult
        {
            public string Xmi { get; set; } = string.Empty;
        }

        [HttpGet("api/item/export_xmi/{workspace}/{itemUri}")]
        public ActionResult<ExportXmiResult> ExportXmi(string workspace, string itemUri)
        {
            workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);
            var foundItem = _workspaceLogic.FindObject(workspace, itemUri);
            if (foundItem == null)
            {
                throw new InvalidOperationException("Item has not been found");
            }

            var provider = new XmiProvider();
            var tempExtent = new MofUriExtent(provider, "dm:///export", _scopeStorage);

            // Now do the copying. it makes us all happy
            var copiedElement = ObjectCopier.Copy(new MofFactory(tempExtent), foundItem, CopyOptions.CopyId);

            return new ExportXmiResult
            {
                Xmi = (
                        (copiedElement as MofElement ?? throw new InvalidOperationException("Not a MofElement"))
                        .ProviderObject as XmiProviderObject
                        ?? throw new InvalidOperationException("not an XmiProviderObject"))
                    .XmlNode.ToString()
            };
        }

        public class ImportXmiParams
        {
            public string Xmi { get; set; } = string.Empty;
        }

        public class ImportXmiResult
        {
            public bool Success { get; set; }
        }

        [HttpPost("api/item/import_xmi/{workspace}/{itemUri}")]
        public async Task<ActionResult<ImportXmiResult>> ImportXmi(
            string workspace, string itemUri,
            [FromQuery(Name = "property")] string property, 
            [FromQuery(Name = "addToCollection")] bool addToCollection,
            [FromBody] ImportXmiParams parameter)
        {
            workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            // Performs the import via the action handler...
            var actionLogic = new ActionLogic(_workspaceLogic, _scopeStorage);
            var importXmi = new ImportXmiActionHandler();
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ImportXmiAction);
            action.set(_DatenMeister._Actions._ImportXmiAction.workspace, workspace);
            action.set(_DatenMeister._Actions._ImportXmiAction.itemUri, itemUri);
            action.set(_DatenMeister._Actions._ImportXmiAction.property, property);
            action.set(_DatenMeister._Actions._ImportXmiAction.addToCollection, addToCollection);
            action.set(_DatenMeister._Actions._ImportXmiAction.xmi, parameter.Xmi);
            
            await importXmi.Evaluate(actionLogic, action);

            return new ImportXmiResult { Success = true };
        }
    }
}