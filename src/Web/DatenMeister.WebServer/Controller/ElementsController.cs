using System.Diagnostics;
using System.Web;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Web.Json;
using DatenMeister.TemporaryExtent;
using DatenMeister.WebServer.Library.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.WebServer.Controller;

[Microsoft.AspNetCore.Components.Route("api/[controller]/[action]")]
[ApiController]
public class ElementsController(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : ControllerBase
{
    public readonly ElementsControllerInternal Internal = new(workspaceLogic, scopeStorage);

    [HttpGet("api/elements/get_name/{workspace}/{extentUri}/{itemId}")]
    public ActionResult<object> GetName(string workspace, string extentUri, string itemId)
    {
        workspace = MvcUrlEncoder.DecodePathOrEmpty(workspace);
        extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);
        itemId = MvcUrlEncoder.DecodePathOrEmpty(itemId);

        var foundItem = workspaceLogic.FindElement(workspace, extentUri, itemId);
        if (foundItem == null)
        {
            return NotFound();
        }

        return ItemWithNameAndId.Create(foundItem)!;
    }

    [HttpGet("api/elements/get_name/{workspace}/{uri}")]
    public ActionResult<ItemWithNameAndId> GetName(string? workspace, string uri)
    {
        workspace = MvcUrlEncoder.DecodePath(workspace);
        uri = MvcUrlEncoder.DecodePathOrEmpty(uri);

        IObject? foundItem;
        if (string.IsNullOrEmpty(workspace) || workspace == "_")
        {
            foundItem = workspaceLogic.FindElement(HttpUtility.UrlDecode(uri));
        }
        else
        {
            foundItem =
                workspaceLogic.GetWorkspace(workspace)?.Resolve(HttpUtility.UrlDecode(uri), ResolveType.NoMetaWorkspaces)
                    as IObject;
        }

        if (foundItem == null)
        {
            return NotFound();
        }

        return ItemWithNameAndId.Create(foundItem)!;
    }

    [HttpGet("api/elements/get_composites/{workspaceId?}/{itemUrl?}")]
    public ActionResult<ItemWithNameAndId[]> GetComposites(string? workspaceId, string? itemUrl)
    {
        workspaceId = MvcUrlEncoder.DecodePath(workspaceId);
        itemUrl = MvcUrlEncoder.DecodePath(itemUrl);

        var result = Internal.GetComposites(workspaceId, itemUrl);
        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    [HttpGet("api/elements/find_by_searchstring")]
    public ActionResult<FindBySearchStringResult> FindBySearchString(string search)
    {
        return Internal.FindBySearchString(search);
    }

    public class FindBySearchStringResult
    {
        public const string ResultTypeNone = "none";
        public const string ResultTypeReference = "reference";
        public const string ResultTypeReferenceExtent = "referenceExtent";

        public string ResultType { get; set; } = ResultTypeNone;

        public ItemWithNameAndId? Reference { get; set; }
    }

    public class CreateTemporaryElementParams
    {
        /// <summary>
        /// Gets or sets the uri of the metaclass
        /// </summary>
        public string? MetaClassUri { get; set; }
    }

    [HttpPut("api/elements/create_temporary_element")]
    public ActionResult<CreateTemporaryElementResult> CreateTemporaryElement([FromBody] CreateTemporaryElementParams? parameter)
    {
        var logic = new TemporaryExtentLogic(workspaceLogic, scopeStorage);

        // Defines the metaclass
        IElement? metaClass = null;
        if (parameter != null && !string.IsNullOrEmpty(parameter.MetaClassUri))
        {
            metaClass =
                workspaceLogic.GetTypesWorkspace()
                    .Resolve(parameter.MetaClassUri, ResolveType.Default) as IElement;
        }

        // Create the temporary element
        var result = logic.CreateTemporaryElement(metaClass);
        return new CreateTemporaryElementResult
        {
            Success = true,
            Id = result.GetId(),
            Workspace = logic.WorkspaceName,
            Uri = result.GetUri() ?? throw new InvalidOperationException("No uri defined"),
            MetaClassUri = result.metaclass?.GetUri() ?? string.Empty,
            MetaClassWorkspace = result.metaclass?.GetUriExtentOf()?.GetWorkspace()?.id ?? string.Empty
        };
    }

    /// <summary>
    /// Defines the result of the CreateTemporaryElement method 
    /// </summary>
    public class CreateTemporaryElementResult
    {
        /// <summary>
        /// Gets or sets a flag indicating the success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the id of the string
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the workspace
        /// </summary>
        public string Workspace { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the uri 
        /// </summary>
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the metaclass Uri which contains the metaclass of the created element
        /// </summary>
        public string MetaClassUri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the workspace of the metaclass which was created by the temporary element
        /// </summary>
        public string MetaClassWorkspace { get; set; } = string.Empty;
    }

    public class QueryObjectParameter
    {
        public MofObjectAsJson Query { get; set; } = new();

        public string DynamicSourceWorkspaceId { get; set; } = string.Empty;
        
        public string DynamicSourceItemUri { get; set; } = string.Empty;

        public int? Timeout = 0;
    }

    [HttpPost("api/elements/query_object")]
    public ActionResult<QueryObjectResult> QueryObject([FromBody] QueryObjectParameter parameter)
    {
        // First, convert the object
        var converter = new MofJsonDeconverter(workspaceLogic, scopeStorage);
        var objectToBeSet = converter.ConvertToObject(parameter.Query)
                            ?? throw new InvalidOperationException("Object to be set is null");
        var resultNode = objectToBeSet.getOrDefault<IElement>(_DataViews._QueryStatement.resultNode)
                         ?? throw new InvalidOperationException("resultNode is not set");

        // Second, execute the query
        var viewLogic = new DataView.DataViewEvaluation(workspaceLogic, scopeStorage);

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        // Sets the maximum allowed timeout
        if (parameter.Timeout > 0 && parameter.Timeout != null)
        {
            viewLogic.MaximumExecutionTiming = TimeSpan.FromSeconds(parameter.Timeout.Value);
        }
        
        // Gets the dynamic source, if set
        if (!string.IsNullOrEmpty(parameter.DynamicSourceWorkspaceId) &&
            !string.IsNullOrEmpty(parameter.DynamicSourceItemUri))
        {

            var foundItem = workspaceLogic.FindExtentAndCollection(
                parameter.DynamicSourceWorkspaceId,
                parameter.DynamicSourceItemUri).collection;
            if (foundItem == null)
            {
                throw new InvalidOperationException(
                    $"Item not found: Workspace ID: {parameter.DynamicSourceWorkspaceId}ItemUri: {parameter.DynamicSourceItemUri}");
            }
            
            viewLogic.AddDynamicSource("input", foundItem);
        }
             
        var resultingNodes = viewLogic.GetElementsForViewNode(resultNode);
        List<object> results = [];

        // Get the items and take care of the allowed timeout
        foreach (var item in resultingNodes)
        {
            if (stopWatch.Elapsed > viewLogic.MaximumExecutionTiming)
            {
                break;
            }

            if (item != null)
            {
                results.Add(item);
            }
        }

        // Third, convert the query result to the json interface
        var result = new QueryObjectResult
        {
            Result = ItemsController.ConvertToJson(results)
        };

        return result;
    }

    public class QueryObjectResult
    {
        /// <summary>
        /// Gets or sets the result of the query as a jsonized string of MofObjects. 
        /// The client side has to convert back the information
        /// </summary>
        public string Result { get; set; } = string.Empty;
    }
}