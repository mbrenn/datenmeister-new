using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Web.Models;
using DatenMeister.Web.Models.PostModels;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/extent")]
    public class ExtentController : ApiController
    {
        /// <summary>
        /// Defines the maximum numnber of items that shall be returned via GetItems
        /// </summary>
        private const int maxItemAmount = 100;

        private readonly IFactoryMapper _mapper;

        public ExtentController(IFactoryMapper mapper)
        {
            _mapper = mapper;
        }

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
        ///     exception is thrown</returns>
        private static Workspace<IExtent> GetWorkspace(string ws)
        {
            var workspace = Core.TheOne.Workspaces.First(x => x.id == ws);
            if (workspace == null)
            {
                throw new InvalidOperationException("Workspace not found");
            }

            return workspace;
        }

        /// <summary>
        /// Returns a list of items being in the query. 
        /// The query contains a filter and a subset of elements
        /// </summary>
        /// <param name="ws">Workspace to be queried</param>
        /// <param name="extent">Extent to be queried</param>
        /// <param name="search">The searchtext being used for query</param>
        /// <param name="o">Offset, defining the index of the first element within the response queue</param>
        /// <param name="a">Number of items being shown</param>
        /// <returns>Enumeration of items</returns>
        [Route("items")]
        public object GetItems(string ws, string extent, string search = null, int o = 0, int a = maxItemAmount)
        {
            var amount = Math.Max(0, Math.Min(100, a)); // Return only the first 100 elements if no index is given
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

            // Perform the filtering
            IEnumerable<object> filteredItems = foundItems;
            if (!string.IsNullOrEmpty(search))
            {
                filteredItems = Filter.WhenOneOfThePropertyContains(
                    foundItems, properties, search, StringComparison.CurrentCultureIgnoreCase);
            }

            // After having the filtered item, we reset the offset

            var filteredAmount = filteredItems.Count();
            if (o < 0)
            {
                // If o is negative, show the last values
                o = filteredAmount + o;
            }
            else
            {
                if (o + amount > filteredAmount)
                {
                    o = filteredAmount - amount;
                }
            }

            filteredItems = filteredItems
                .Skip(o)
                .Take(amount);

            // Now return our stuff
            var result = new ExtentContentModel
            {
                url = extent,
                columns = properties
                    .Select(x => new DataTableColumn
                    {
                        name = x.ToString(),
                        title = x.ToString()
                    })
                    .ToList(),
                totalItemCount = totalItems.Count(),
                search = search,
                filteredItemCount = filteredAmount,
                items = filteredItems
                    .Select(x => new DataTableItem
                    {
                        uri = foundExtent.uri(x as IElement),
                        v = (x as IElement).AsStringDictionary(properties)
                    })
                    .ToList()
            };

            return result;
        }

        [Route("item")]
        public object GetItem(string ws, string extent, string item)
        {
            Workspace<IExtent> foundWorkspace;
            IUriExtent foundExtent;
            RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

            var itemModel = new ItemContentModel {uri = item};

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

        [Route("item_create")]
        [HttpPost]
        public object CreateItem([FromBody] ItemCreateModel model)
        {
            Workspace<IExtent> foundWorkspace;
            IUriExtent foundExtent;
            RetrieveWorkspaceAndExtent(model.ws, model.extent, out foundWorkspace, out foundExtent);

            if (!string.IsNullOrEmpty(model.container))
            {
                throw new InvalidOperationException("Element creation within container is not supported.");
            }

            var factory = _mapper.FindFactoryFor(foundExtent);
            var element = factory.create(null);

            foundExtent.elements().add(element);
            var newUrl = foundExtent.uri(element);
            
            return new
            {
                success = true,
                newuri = newUrl
            };
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
        private static ExtentModel GetExtentModel(string ws, string extent)
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
        private static void RetrieveWorkspaceAndExtent(
            ItemReferenceModel model,
            out Workspace<IExtent> foundWorkspace,
            out IUriExtent foundExtent)
        {
            RetrieveWorkspaceAndExtent(model.ws, model.extent, out foundWorkspace, out foundExtent);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private static void RetrieveWorkspaceAndExtent(
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
        private static void FindItem(
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