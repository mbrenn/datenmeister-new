// Configuration parameter which limits the number of elements

#if DEBUG
#define LimitNumberOfElements
#endif

using System.Text;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Web.Json;
using DatenMeister.Provider.ExtentManagement;
using DatenMeister.WebServer.Library.Helper;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DatenMeister.WebServer.Controller;

[Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
[ApiController]
public class ItemsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : ControllerBase
{
    private readonly ItemsControllerInternal _internal = new(workspaceLogic, scopeStorage);

    [HttpPost("api/items/create_in_extent")]
    public ActionResult<object> CreateItemInExtent(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "e")] string extentUri,
        [FromBody] CreateItemInExtentParams createParams)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);

        var extent = workspaceLogic.FindExtent(workspaceId, extentUri)
                     ?? throw new InvalidOperationException("Extent is not found");

        var factory = new MofFactory(extent);

        var metaClass =
            string.IsNullOrEmpty(createParams.MetaClass)
                ? null
                : extent.GetUriResolver().Resolve(createParams.MetaClass, ResolveType.IncludeMetaWorkspaces) as IElement;

        var item = factory.create(metaClass);
        var values = createParams.Properties?.v;
        if (values != null)
        {
            var jsonConverter = new DirectJsonDeconverter(workspaceLogic, scopeStorage);
            foreach (var (key, valueObject) in values)
            {
                var valueAsArray = (jsonConverter.ConvertJsonValue(valueObject) as IEnumerable<object?>)?.ToArray();
                if (valueAsArray == null)
                {
                    throw new InvalidOperationException("Value is not an array");
                }
                var isSet = DotNetHelper.AsBoolean(valueAsArray[0]);
                var value = valueAsArray[1];
                if (isSet)
                {
                    var propertyValue = jsonConverter.ConvertJsonValue(value);

                    if (propertyValue != null) item.set(key, propertyValue);
                }
                else
                {
                    item.unset(key);
                }
            }
        }

        extent.elements().add(item);

        return new CreateItemInExtentResult
        {
            Success = true,
            ItemId = (item as IHasId)?.Id ?? string.Empty,
            Workspace = workspaceId,
            ItemUrl = item.GetUri() ?? string.Empty
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
        public string? MetaClass { get; set; }

        public MofObjectAsJson? Properties { get; set; }
    }

    public class CreateItemInExtentResult
    {
        public bool Success { get; set; }
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the item url
        /// </summary>
        public string ItemUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the workspace
        /// </summary>
        public string Workspace { get; set; } = string.Empty;
    }

    [HttpPost("api/items/create_child")]
    public ActionResult<object> CreateItemAsChild(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromBody] CreateItemAsChildParams createItemAsParams)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var item = _internal.GetItemByUriParameter(workspaceId, itemUri);

        var factory = new MofFactory(item);

        var metaClass =
            string.IsNullOrEmpty(createItemAsParams.MetaClass)
                ? null
                : (item.GetUriResolver() ?? throw new InvalidOperationException("No UriResolver"))
                .Resolve(createItemAsParams.MetaClass, ResolveType.IncludeMetaWorkspaces) as IElement;

        var child = factory.create(metaClass);
        if (createItemAsParams.AsList)
            item.AddCollectionItem(createItemAsParams.Property, child);
        else
            item.set(createItemAsParams.Property, child);

        var values = createItemAsParams.Properties?.v;
        if (values != null)
        {
            var jsonConverter = new DirectJsonDeconverter(workspaceLogic, scopeStorage);
            foreach (var (key, valueObject) in values)
            {
                var valueAsArray = (jsonConverter.ConvertJsonValue(valueObject) as IEnumerable<object?>)?.ToArray();
                if (valueAsArray == null)
                {
                    throw new InvalidOperationException("Value is not an array");
                }

                var isSet = DotNetHelper.AsBoolean(valueAsArray[0]);
                var value = valueAsArray[1];
                if (isSet)
                {
                    var propertyValue = jsonConverter.ConvertJsonValue(value);

                    if (propertyValue != null) child.set(key, propertyValue);
                }
            }
        }

        return new CreateItemAsChildResult
        {
            Success = true,
            ItemId = (child as IHasId)?.Id ?? string.Empty,
            Workspace = workspaceId,
            ItemUrl = child.GetUri() ?? string.Empty
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
        public string MetaClass { get; set; } = string.Empty;

        /// <summary>
        /// Property in which the item shall be added
        /// </summary>
        public string Property { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether the child shall be added as a list item or
        /// shall be directly set to the property
        /// </summary>
        public bool AsList { get; set; }

        public MofObjectAsJson? Properties { get; set; }
    }

    public class CreateItemAsChildResult
    {
        public bool Success { get; set; }
        public string ItemId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the item url
        /// </summary>
        public string ItemUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the workspace
        /// </summary>
        public string Workspace { get; set; } = string.Empty;
    }

    [HttpDelete("api/items/delete")]
    public ActionResult<object> DeleteItem(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUrl)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUrl = MvcUrlEncoder.DecodePathOrEmpty(itemUrl);

        var success = false;
        var foundItem = workspaceLogic.FindObject(workspaceId, itemUrl);
        if (foundItem != null) success = ObjectHelper.DeleteObject(foundItem);

        return new { success };
    }

    /// <summary>
    /// Deletes all elements from an extent
    /// </summary>
    /// <param name="workspaceId">Id of the workspace in which the extent is residing</param>
    /// <param name="extentUri">Uri of the extent</param>
    /// <returns>The action result</returns>
    [HttpDelete("api/items/delete_root_elements")]
    public ActionResult<SuccessResult> DeleteRootElements(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "e")] string extentUri)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);

        var success = true;
        var extent = workspaceLogic.FindExtent(workspaceId, extentUri);
        if (extent == null)
        {
            return NotFound("Extent is not found");
        }

        extent.elements().RemoveAll();

        return new SuccessResult { Success = success };
    }

    /// <summary>
    /// Deletes an item from the extent itself
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="itemUri">Uri of the item to be deleted</param>
    /// <returns>the value indicating the success or not</returns>
    [HttpPost("api/items/delete_from_extent")]
    public ActionResult<object> DeleteItemFromExtent(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri)
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
        return new { success = true };
    }

    [HttpGet("api/items/get_by_id")]
    public ActionResult<object> GetItem(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "e")] string extentUri,
        [FromQuery(Name = "i")] string itemId)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);
        itemId = MvcUrlEncoder.DecodePathOrEmpty(itemId);

        var foundElement = _internal.GetItemInternal(workspaceId, extentUri, itemId, out var converter);
        var convertedElement = converter.ConvertToJsonString(foundElement);

        return convertedElement;
    }

    [HttpGet("api/items/get")]
    public ActionResult<object> GetItem(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var foundElement = _internal.GetItemByUriParameter(workspaceId, itemUri);

        var converter = new MofJsonConverter { MaxRecursionDepth = 2, ResolveCompositesRecursively = true };
        var convertedElement = converter.ConvertToJsonString(foundElement);

        return convertedElement;
    }

    [HttpGet("api/items/get_itemwithnameandid")]
    public ActionResult<object> GetItemWithNameAndId(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var foundElement = _internal.GetItemByUriParameter(workspaceId, itemUri);

        return ItemWithNameAndId.Create(foundElement)!;
    }

    public class GetRootElementsResult
    {
        public bool Success { get; init; }
        public string RootElements { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }

    /// <summary>
    /// Gets the root elements of a certain extent and workspace
    /// </summary>
    /// <param name="workspaceId">Id of the workspace</param>
    /// <param name="extentUri">Uri of the extent from which the items are retrieved</param>
    /// <param name="viewNode">The view node being used to filter the items</param>
    /// <param name="orderBy">Name of the property</param>
    /// <param name="orderByDescending">true, if a descending order shall be applied</param>
    /// <param name="filterByProperties">Serialized properties to be set</param>
    /// <param name="filterByFreeText">Gets or sets the value whether the free text</param>
    /// <returns></returns>
    [HttpGet("api/items/get_root_elements")]
    public ActionResult<GetRootElementsResult> GetRootElements(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "e")] string extentUri,
        [FromQuery] string? viewNode = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool? orderByDescending = false,
        [FromQuery] string? filterByProperties = null,
        [FromQuery] string? filterByFreeText = null,
        [FromQuery] string? columnsIncludeOnly = null,
        [FromQuery] string? columnsExclude = null)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);
        viewNode = MvcUrlEncoder.DecodePath(viewNode);

        var query = new QueryFilterParameter
        {
            OrderBy = orderBy,
            OrderByDescending = orderByDescending == true,
            FilterByFreeText = filterByFreeText,
            FilterByProperties = ItemsControllerInternal.DeserializeStringToDictionary(filterByProperties ?? ""),
            ColumnsIncludeOnly = columnsIncludeOnly,
            ColumnsExclude = columnsExclude
        };

        var result = _internal.GetRootElementsInternal(workspaceId, extentUri, viewNode, query);
        var finalElements = result.Elements;
        if (finalElements == null)
        {
            return NotFound();
        }

        return new ActionResult<GetRootElementsResult>(new GetRootElementsResult
        {
            RootElements =  ConvertToJson(finalElements),
            Success = true,
            Message  = result.IsCapped ? "The result is capped to 100 items" : string.Empty,
        });
    }

    /// <summary>
    /// Converts the given list of elements to a json array
    /// </summary>
    /// <param name="finalElements">Final elements to be converted</param>
    /// <returns>The json as string</returns>
    public static string ConvertToJson(IEnumerable<object?> finalElements)
    {
        var converter = new MofJsonConverter { MaxRecursionDepth = 2 };

        var result = new StringBuilder();
        result.Append('[');
        var komma = string.Empty;

        foreach (var item in finalElements)
        {
            result.Append(komma);
            result.Append(converter.ConvertToJsonString(item));

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
    [HttpGet("api/items/get_elements")]
    public ActionResult<string> GetElements([FromQuery] string queryUri)
    {
        queryUri = MvcUrlEncoder.DecodePathOrEmpty(queryUri);
        var result = workspaceLogic.Resolve(queryUri, ResolveType.Default, true);
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
    [HttpGet("api/items/get_root_elements_as_item")]
    public ActionResult<IEnumerable<ItemWithNameAndId>> GetRootElementsAsItem(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "e")] string extentUri,
        [FromQuery] string? viewNode = null)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);
        viewNode = MvcUrlEncoder.DecodePath(viewNode);

        var finalElements = _internal.GetRootElementsInternal(workspaceId, extentUri, viewNode);
        if (finalElements.Elements == null)
        {
            return NotFound($"{extentUri} did not return a reflective collection");
        }

        return finalElements.Elements.Select(x => ItemWithNameAndId.Create(x)!).ToList();
    }

    /// <summary>
    /// Gets the elements as items as returned by the query
    /// </summary>
    /// <param name="queryUri">Uri to be queried</param>
    /// <returns>Enumeration of items being queried</returns>
    [HttpGet("api/items/get_elements_as_item")]
    public ActionResult<IEnumerable<ItemWithNameAndId>> GetElementsAsItem([FromQuery] string queryUri)
    {
        queryUri = MvcUrlEncoder.DecodePathOrEmpty(queryUri);
        var result = workspaceLogic.Resolve(queryUri, ResolveType.Default, true);
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

    [HttpGet("api/items/get_container")]
    public ActionResult<List<ItemWithNameAndId>> GetContainer(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromQuery] bool? self = false)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);
        var result = new List<ItemWithNameAndId>();

        IUriExtent? extent;

        // Checks, if we have found the item
        var foundItem = workspaceLogic.FindObjectOrCollection(workspaceId, itemUri);

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

            var managementWorkspaceItem = workspaceLogic.GetManagementWorkspace()
                .ResolveById(ExtentManagementHelper.GetIdOfWorkspace(workspace.id));
            if (managementWorkspaceItem != null)
            {
                result.Add(ItemWithNameAndId.Create(managementWorkspaceItem, EntentType.Workspace)
                           ?? throw new InvalidOperationException("Should not happen"));
            }
        }

        return result;
    }

    [HttpPut("api/items/set_property")]
    public ActionResult<object> SetProperty(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromBody] SetPropertyParams propertyParams)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                        ?? throw new InvalidOperationException("Item was not found");
        foundItem.set(propertyParams.Key, propertyParams.Value);
        return new { success = true };
    }

    [HttpPost("api/items/set_property_reference")]
    public ActionResult<SuccessResult> SetPropertyReference(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromBody] SetPropertyReferenceParams parameters)
    {

        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri);

        var reference = _internal.GetItemByUriParameter(
            parameters.WorkspaceId,
            parameters.ReferenceUri);

        foundItem.set(parameters.Property, reference);

        return new SuccessResult() { Success = true };
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

    [HttpPost("api/items/unset_property")]
    public ActionResult<object> UnsetProperty(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromBody] UnsetPropertyParams propertyParams)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                        ?? throw new InvalidOperationException("Item was not found");
        foundItem.unset(propertyParams.Property);

        return new { success = true };
    }

    /// <summary>
    ///     Defines the property for the unset property
    /// </summary>
    public class UnsetPropertyParams
    {
        /// <summary>
        ///     Gets or sets the key
        /// </summary>
        public string Property { get; init; } = string.Empty;
    }

    [HttpPost("api/items/set_properties")]
    public ActionResult<object> SetProperties(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
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

        return new { success = true };
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

    [HttpGet("api/items/get_property")]
    public ActionResult<object> GetProperty(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromQuery] string property)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);
        
        var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                        ?? throw new InvalidOperationException("Item was not found");
        
        var value = foundItem.get(property);
        var isSet = foundItem.isSet(property);
        
        var converter = new MofJsonConverter { MaxRecursionDepth = 2, ResolveReferenceToOtherExtents = true };
        // return an array of two values. The first value whether the element is set, the second value the element itself
        return Content($"{{\"v\": [{converter.ConvertToJsonString(isSet)},{converter.ConvertToJsonString(value)}]}}", "application/json", Encoding.UTF8);
    }

    [HttpPut("api/items/set")]
    public ActionResult<SuccessResult> Set(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromBody] MofObjectAsJson jsonObject)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var converter = new MofJsonDeconverter(workspaceLogic, scopeStorage);
        var objectToBeSet = converter.ConvertToObject(jsonObject);
        if (objectToBeSet == null)
        {
            return NotFound("Should not be null");
        }

        var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri);
        ObjectCopier.CopyPropertiesStatic(objectToBeSet, foundItem);

        return new SuccessResult { Success = true };
    }

    [HttpPost("api/items/set_metaclass")]
    public ActionResult<object> SetMetaClass(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
        [FromBody] SetMetaClassParams parameter)
    {
        workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var foundItem = _internal.GetItemByUriParameter(workspaceId, itemUri)
                        ?? throw new InvalidOperationException("Item was not found");
        if (foundItem is IElementSetMetaClass asMetaClassSet)
            asMetaClassSet.SetMetaClass(new MofObjectShadow(parameter.MetaClass));

        return new { success = true };
    }


    public class SetMetaClassParams
    {
        public string MetaClass { get; set; } = string.Empty;
    }

    /// <summary>
    ///     Adds a reference to a property's collection
    /// </summary>
    /// <param name="workspaceId">Id of the workspace where the item can be found</param>
    /// <param name="itemUri">Uri of the item to whose property shall be added</param>
    /// <param name="parameters">Parameters describing the property and the reference</param>
    /// <returns></returns>
    [HttpPost("api/items/add_ref_to_collection")]
    public ActionResult<object> AddReferenceToCollection(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
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

        return new { success = true };
    }

    /// <summary>
    ///     Defines the parameters to add a reference to a property's collection.
    /// </summary>
    public class AddReferenceToCollectionParams
    {
        /// <summary>
        ///     Defines the property to which the property will be added
        /// </summary>
        public string Property { get; init; } = string.Empty;

        /// <summary>
        /// Defines the workspace from which the property will be laoded
        /// </summary>
        public string? WorkspaceId { get; init; }

        /// <summary>
        /// Defines the reference Uri from which the property will be loaded
        /// </summary>
        public string ReferenceUri { get; init; } = string.Empty;
    }

    /// <summary>
    ///     Adds a reference to a property's collection
    /// </summary>
    /// <param name="workspaceId">Id of the workspace where the item can be found</param>
    /// <param name="itemUri">Uri of the item to whose property shall be added</param>
    /// <param name="parameters">Parameters describing the property and the reference</param>
    /// <returns></returns>
    [HttpPost("api/items/remove_ref_to_collection")]
    public ActionResult<object> RemoveReferenceToCollection(
        [FromQuery(Name = "w")] string workspaceId,
        [FromQuery(Name = "u")] string itemUri,
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

        return new { success = true };
    }

    public class RemoveReferenceToCollectionParams
    {
        /// <summary>
        ///     Defines the property to which the property will be removed
        /// </summary>
        public string Property { get; init; } = string.Empty;

        public string? WorkspaceId { get; init; }

        public string ReferenceUri { get; init; } = string.Empty;
    }


    public class ExportXmiResult
    {
        public string Xmi { get; set; } = string.Empty;
    }

    [HttpGet("api/items/export_xmi")]
    public ActionResult<ExportXmiResult> ExportXmi(
        [FromQuery(Name = "w")] string workspace,
        [FromQuery(Name = "u")] string itemUri)
    {
        workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);
        var foundItem = workspaceLogic.FindObject(workspace, itemUri);
        if (foundItem == null)
        {
            throw new InvalidOperationException("Item has not been found");
        }

        var provider = new XmiProvider();
        var tempExtent = new MofUriExtent(provider, "dm:///export", scopeStorage);

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
        public string Xmi { get; init; } = string.Empty;
    }

    public class ImportXmiResult
    {
        public bool Success { get; init; }
    }

    [HttpPost("api/items/import_xmi")]
    public async Task<ActionResult<ImportXmiResult>> ImportXmi(
        [FromQuery(Name = "w")] string workspace,
        [FromQuery(Name = "u")] string itemUri,
        [FromQuery(Name = "property")] string property,
        [FromQuery(Name = "addToCollection")] bool addToCollection,
        [FromBody] ImportXmiParams parameter)
    {
        workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        // Performs the import via the action handler...
        var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
        var importXmi = new ImportXmiActionHandler();
        var action = InMemoryObject.CreateEmpty(_Actions.TheOne.__ImportXmiAction);
        action.set(_Actions._ImportXmiAction.workspaceId, workspace);
        action.set(_Actions._ImportXmiAction.itemUri, itemUri);
        action.set(_Actions._ImportXmiAction.property, property);
        action.set(_Actions._ImportXmiAction.addToCollection, addToCollection);
        action.set(_Actions._ImportXmiAction.xmi, parameter.Xmi);

        await importXmi.Evaluate(actionLogic, action);

        return new ImportXmiResult { Success = true };
    }

    public class SetIdParams
    {
        public string Id { get; init; } = string.Empty;
    }

    public class SetIdResult
    {
        public bool Success { get; init; }

        public string NewUri { get; init; } = string.Empty;
    }

    /// <summary>
    /// Sets the id of the item via the API
    /// </summary>
    /// <param name="workspace">Workspace in which the item resides</param>
    /// <param name="itemUri">Uri of the item whose id shall be changed</param>
    /// <param name="parameter">Defines the new Id of the item</param>
    /// <returns></returns>
    [HttpPost("api/items/set_id")]
    public ActionResult<SetIdResult> SetId(
        [FromQuery(Name = "w")] string workspace,
        [FromQuery(Name = "u")] string itemUri,
        [FromBody] SetIdParams parameter)
    {
        workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
        itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

        var foundItem = workspaceLogic.FindObject(workspace, itemUri);
        switch (foundItem)
        {
            case null:
                return NotFound(new SetIdResult { Success = false });
            case ICanSetId asCanSetId:
                asCanSetId.Id = parameter.Id;
                return new SetIdResult { Success = true, NewUri = foundItem.GetUri() ?? string.Empty };
            default:
                return NotFound(new SetIdResult { Success = false });
        }
    }
}