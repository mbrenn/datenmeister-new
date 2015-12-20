using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Models;
using DatenMeister.Web.Models.PostModels;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/extent")]
    public class ExtentController : ApiController
    {
        [Route("all")]
        public object GetAll(string ws)
        {
            var result = new List<object>();
            var workspace = GetWorkspace(ws);

            foreach (var extent in workspace.extent.Cast<IUriExtent>())
            {
                result.Add(
                    new
                    {
                        uri = extent.contextURI(),
                        count = extent.elements().Count()
                    });
            }

            return result;
        }

        /// <summary>
        ///     Gets the workspace by name, if workspace is not found, an exception
        ///     will be thrown
        /// </summary>
        /// <param name="ws">Name of the workspace to be queried</param>
        /// <returns>
        ///     The found workspace. If the workspace is not found, an
        ///     exception is thrown/returns>
        private static Workspace<IExtent> GetWorkspace(string ws)
        {
            var workspace = Core.TheOne.Workspaces.First(x => x.id == ws);
            if (workspace == null)
            {
                throw new InvalidOperationException("Workspace not found");
            }

            return workspace;
        }

        [Route("items")]
        public object GetItems(string ws, string extent)
        {
            var amount = 100; // Return only the first 100 elements if no index is given
            var offset = 0;
            var workspace = GetWorkspace(ws);
            var foundExtent =
                workspace.extent
                    .Cast<IUriExtent>()
                    .FirstOrDefault(x => x.contextURI() == extent);
            if (foundExtent == null)
            {
                throw new InvalidOperationException("Not found");
            }

            var totalItems = foundExtent.elements();
            var foundItems = totalItems;

            var properties = foundExtent.GetProperties().ToList();

            var result = new ExtentContentModel();
            result.url = extent;
            result.columns = properties
                .Select(x => new DataTableColumn
                {
                    name = x.ToString(),
                    title = x.ToString()
                })
                .ToList();
            result.totalItemCount = totalItems.Count();
            result.filteredItemCount = foundItems.Count();
            result.items = totalItems
                .Skip(offset)
                .Take(amount)
                .Select(x => new DataTableItem
                {
                    uri = foundExtent.uri(x as IElement),
                    v = (x as IElement).AsStringDictionary(properties)
                })
                .ToList();

            return result;
        }

        [Route("item")]
        public object GetItem(string ws, string extent, string item)
        {
            Workspace<IExtent> foundWorkspace;
            IUriExtent foundExtent;
            RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

            var itemModel = new ItemContentModel();
            itemModel.uri = item;
            

            // Retrieves the values of the item
            var foundElement = foundExtent.element(item);
            if (foundElement == null)
            {
                // Not found
                return NotFound();
            }

            var foundProperties = foundExtent.GetProperties();
            foreach (var property in foundProperties)
            {
                if (foundElement.isSet(property))
                {
                    itemModel.v[property.ToString()] = foundElement.get(property)?.ToString();
                }
            }

            return itemModel;
        }

        [Route("item_delete")]
        [HttpPost]
        public object DeleteItem([FromBody] ItemDeleteModel model)
        {
            try
            {
                Workspace<IExtent> foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                FindItem(model, out foundWorkspace, out foundExtent, out foundItem);

                if (!foundExtent.elements().remove(foundItem))
                {
                    return NotFound();
                }

                return new {success = true};
            }
            catch (OperationFailedException)
            {
                return NotFound();
            }
        }

        [Route("item_unset_property")]
        [HttpPost]
        public object UnsetPropertyValue([FromBody] ItemUnsetPropertyModel model)
        {
            try
            {
                Workspace<IExtent> foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                FindItem(model, out foundWorkspace, out foundExtent, out foundItem);

                foundItem.unset(model.property);

                return new {success = true};
            }
            catch (OperationFailedException)
            {
                return NotFound();
            }
        }

        [Route("item_set_property")]
        [HttpPost]
        public object SetPropertyValue([FromBody] ItemSetPropertyModel model)
        {
            try
            {
                Workspace<IExtent> foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                FindItem(model, out foundWorkspace, out foundExtent, out foundItem);

                foundItem.set(model.property, model.newValue);

                return new {success = true};
            }
            catch (OperationFailedException)
            {
                return NotFound();
            }
        }

        [Route("item_set_properties")]
        [HttpPost]
        public object SetPropertiesValue([FromBody] ItemSetPropertiesModel model)
        {
            try
            {
                Workspace<IExtent> foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                FindItem(model, out foundWorkspace, out foundExtent, out foundItem);

                foreach (var pair in model.v)
                {
                    foundItem.set(pair.Key, pair.Value);
                }

                return new {success = true};
            }
            catch (OperationFailedException)
            {
                return NotFound();
            }
        }

        /// <summary>
        ///     Gets the extentmodel for a given extent
        /// </summary>
        /// <param name="ws">Workspace to be queried</param>
        /// <param name="extent">Extent to be querued</param>
        /// <returns>The found extent or an exception.</returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public static ExtentModel GetExtentModel(string ws, string extent)
        {
            Workspace<IExtent> foundWorkspace;
            IUriExtent foundExtent;
            RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

            var extentModel = new ExtentModel(
                foundExtent,
                new WorkspaceModel(foundWorkspace));

            return extentModel;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static void RetrieveWorkspaceAndExtent(
            ItemReferenceModel model,
            out Workspace<IExtent> foundWorkspace,
            out IUriExtent foundExtent)
        {
            RetrieveWorkspaceAndExtent(model.ws, model.extent, out foundWorkspace, out foundExtent);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static void RetrieveWorkspaceAndExtent(
            string ws,
            string extent,
            out Workspace<IExtent> foundWorkspace,
            out IUriExtent foundExtent)
        {
            foundWorkspace = Core.TheOne.Workspaces.FirstOrDefault(x => x.id == ws);

            if (foundWorkspace == null)
            {
                throw new OperationFailedException("Workspace_NotFound");
            }

            foundExtent = foundWorkspace.extent.Cast<IUriExtent>().FirstOrDefault(x => x.contextURI() == extent);
            if (foundExtent == null)
            {
                throw new OperationFailedException("Extent_NotFound");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public static void FindItem(
            ItemReferenceModel model,
            out Workspace<IExtent> foundWorkspace,
            out IUriExtent foundExtent,
            out IElement foundItem)
        {
            RetrieveWorkspaceAndExtent(model, out foundWorkspace, out foundExtent);

            foundItem = foundExtent.element(model.item);
            if (foundItem == null)
            {
                throw new OperationFailedException();
            }
        }
    }
}