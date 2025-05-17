using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Web.Json;
using DatenMeister.WebServer.Library.Helper;
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
        public ActionResult<string> Get(string formUri, string? formType)
        {
            formUri = MvcUrlEncoder.DecodePathOrEmpty(formUri);
            var form = _internal.GetInternal(formUri);

            // Performs a friendly conversion from the actual form type to the requested form type
            if (formType != null)
            {
                if(Enum.TryParse<_DatenMeister._Forms.___FormType>(formType, true, out var result))
                {
                    form = FormMethods.ConvertFormToObjectOrCollectionForm(
                        form,
                        result);
                }
            }

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/default_for_item/{workspaceId}/{itemUrl}/{viewMode?}")]
        public ActionResult<string> GetDefaultForItem(string workspaceId, string itemUrl, string? viewMode)
        {
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUrl = MvcUrlEncoder.DecodePathOrEmpty(itemUrl);
            viewMode = MvcUrlEncoder.DecodePath(viewMode);

            var form = _internal.GetObjectFormForItemInternal(workspaceId, itemUrl, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/default_for_extent/{workspaceId}/{extentUri}/{viewMode?}")]
        public ActionResult<string> GetDefaultForExtent(string workspaceId, string extentUri, string? viewMode)
        {
            viewMode = MvcUrlEncoder.DecodePath(viewMode);
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);

            var form = _internal.GetCollectionFormForExtentInternal(workspaceId, extentUri, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        /// <summary>
        /// Defines the result for the function Create Collection Form For Extent
        /// </summary>
        public class CreateCollectionFormForExtentResult
        {
            /// <summary>
            /// The uri and name of the created form
            /// </summary>
            public ItemWithNameAndId? CreatedForm { get; set; }
        }

        /// <summary>
        /// Creates a collection form for by defining the workspace, extent and viewmode.
        /// The form will be stored in the management extent
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUri">Uri of the extent itself</param>
        /// <param name="viewMode">Viewmode of the extent</param>
        /// <returns></returns>
        [HttpPost("api/forms/create_collection_form_for_extent/{workspaceId}/{extentUri}/{viewMode?}")]
        public ActionResult<CreateCollectionFormForExtentResult> CreateCollectionFormForExtent(string workspaceId,
            string extentUri, string? viewMode)
        {
            var formMethods = new FormMethods(_internal.WorkspaceLogic, _internal.ScopeStorage);
            var formFactory = new FormFactory(_internal.WorkspaceLogic, _internal.ScopeStorage);

            viewMode = MvcUrlEncoder.DecodePath(viewMode);
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            extentUri = MvcUrlEncoder.DecodePathOrEmpty(extentUri);

            var factory = new MofFactory(formMethods.GetFormExtent(FormLocationType.User));

            var (collection, extent) = _internal.WorkspaceLogic.FindExtentAndCollection(workspaceId, extentUri);
            if (extent == null || collection == null)
            {
                throw new InvalidOperationException($"Extent not found: {workspaceId} - {extentUri}");

            }

            // Creates the form itself
            var form = formFactory.CreateCollectionFormForExtent(
                extent,
                collection,
                new FormFactoryConfiguration
                {
                    ViewModeId = viewMode ?? string.Empty,
                    Factory = factory
                })
                ?? throw new InvalidOperationException("Form returned null for whatever reason");

            formMethods.GetUserFormExtent().elements().add(form);
            
            return new CreateCollectionFormForExtentResult
            {
                CreatedForm = ItemWithNameAndId.Create(form)
            };
        }

        /// <summary>
        /// Defines the result for the function Create Collection Form For Extent
        /// </summary>
        public class CreateObjectFormForItemResult
        {
            /// <summary>
            /// The uri and name of the created form
            /// </summary>
            public ItemWithNameAndId? CreatedForm { get; set; }
        }


        /// <summary>
        /// Creates a collection form for by defining the workspace, extent and viewmode.
        /// The form will be stored in the management extent
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="itemUri">Uri of the item itself</param>
        /// <param name="viewMode">Viewmode of the extent</param>
        /// <returns></returns>
        [HttpPost("api/forms/create_object_form_for_item/{workspaceId}/{itemUri}/{viewMode?}")]
        public ActionResult<CreateObjectFormForItemResult> CreateObjectFormForItem(
            string workspaceId, string itemUri, string? viewMode)
        {
            var formMethods = new FormMethods(_internal.WorkspaceLogic, _internal.ScopeStorage);
            var formFactory = new FormFactory(_internal.WorkspaceLogic, _internal.ScopeStorage);

            viewMode = MvcUrlEncoder.DecodePath(viewMode);
            workspaceId = MvcUrlEncoder.DecodePathOrEmpty(workspaceId);
            itemUri = MvcUrlEncoder.DecodePathOrEmpty(itemUri);

            var userFormExtent = formMethods.GetUserFormExtent();
            var factory = new MofFactory(userFormExtent);

            var element = _internal.WorkspaceLogic.FindObject(workspaceId, itemUri);
            if (element == null)
            {
                throw new InvalidOperationException($"Extent not found: {workspaceId} - {itemUri}");
            }

            // Creates the form itself
            var form = formFactory.CreateObjectFormForItem(
                element,
                new FormFactoryConfiguration
                {                    
                    ViewModeId = viewMode ?? string.Empty,
                    Factory = factory, 
                    ViaFormFinder = false // We want to create a complete and fresh form
                }) ?? throw new InvalidOperationException("Form returned null for whatever reason");

            userFormExtent.elements().add(form);

            return new CreateObjectFormForItemResult
            {
                CreatedForm = ItemWithNameAndId.Create(form)
            };
        }


        [HttpGet("api/forms/default_object_for_metaclass/{metaClass?}/{viewMode?}")]
        public ActionResult<string> GetDefaultObjectForMetaClass(string? metaClass, string? viewMode) 
        {
            viewMode = MvcUrlEncoder.DecodePath(viewMode);
            metaClass = MvcUrlEncoder.DecodePath(metaClass);
                        
            if (metaClass == "_")
            {
                // Converts back the underscore to undefined value
                metaClass = null;
            }

            var form = _internal.GetObjectFormForMetaClassInternal(metaClass, viewMode);

            return MofJsonConverter.ConvertToJsonWithDefaultParameter(form);
        }

        [HttpGet("api/forms/get_viewmodes")]
        public ActionResult<GetViewModesResult> GetViewModes()
        {
            var viewModes = _internal.GetViewModesInternal();
            return new GetViewModesResult
            {
                ViewModes = viewModes
                    .Select(x=> MofJsonConverter.ConvertToJsonWithDefaultParameter(x))
                    .ToList()
            };
        }

        public record GetViewModesResult
        {
            /// <summary>
            /// A list of view modes
            /// </summary>
            public List<string> ViewModes { get; set; } = [];
        }

        [HttpGet("api/forms/get_default_viewmode/{workspaceId}/{extentUri}")]
        public ActionResult<GetDefaultViewModeResult> GetDefaultViewMode(string? workspaceId, string? extentUri)
        {
            workspaceId = MvcUrlEncoder.DecodePath(workspaceId);
            extentUri = MvcUrlEncoder.DecodePath(extentUri);
            if (workspaceId == null) throw new ArgumentNullException(nameof(workspaceId));
            if (extentUri == null) throw new ArgumentNullException(nameof(extentUri));

            var formMethods = new FormMethods(_internal.WorkspaceLogic, _internal.ScopeStorage);
            var extent = _internal.WorkspaceLogic.FindExtent(workspaceId, extentUri);

            var viewMode = formMethods.GetDefaultViewMode(extent);
            if (viewMode == null)
            {
                throw new InvalidOperationException("No viewmode was found");
            }

            return new GetDefaultViewModeResult()
            {
                ViewMode = MofJsonConverter.ConvertToJsonWithDefaultParameter(viewMode)
            };
        }

        public record GetDefaultViewModeResult
        {
            /// <summary>
            /// A list of view modes
            /// </summary>
            public string ViewMode { get; set; } = ViewModes.Default;
        }
    }
}