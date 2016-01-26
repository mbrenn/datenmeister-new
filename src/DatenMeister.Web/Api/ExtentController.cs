using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;
using DatenMeister.Runtime;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Web.Helper;
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
        private readonly IWorkspaceCollection _workspaceCollection;

        public ExtentController(IFactoryMapper mapper, IWorkspaceCollection workspaceCollection)
        {
            _mapper = mapper;
            _workspaceCollection = workspaceCollection;
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
        private Workspace<IExtent> GetWorkspace(string ws)
        {
            var workspace = _workspaceCollection.Workspaces.First(x => x.id == ws);
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

            var columnCreator = new ColumnCreator();
            var columns = columnCreator.GetColumnsForTable(foundExtent).ToList();
            var properties = columnCreator.Properties;

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
                columns = columns,
                totalItemCount = totalItems.Count(),
                search = search,
                filteredItemCount = filteredAmount,
                items = filteredItems
                    .Select(x => new DataTableItem
                    {
                        uri = foundExtent.uri(x as IElement),
                        v = ConvertToJson(x as IElement, columnCreator)
                    })
                    .ToList()
            };

            return result;
        }

        private static Dictionary<string, object> ConvertToJson(IObject element, ColumnCreator creator)
        {
            var result = new Dictionary<string, object>();

            foreach (var property in creator.Properties
                .Where(property => element.isSet(property)))
            {
                var propertyAsString = property.ToString();
                var propertyValue = element.get(property);

                if (creator.ColumnsOnProperty[property].isEnumeration)
                {
                    if (propertyValue is IEnumerable && !(propertyValue is string))
                    {
                        var list = new List<object>();
                        foreach (var listValue in (propertyValue as IEnumerable))
                        {
                            list.Add(new
                            {
                                u = "xyz",
                                v = listValue == null ? "null" : listValue.ToString()
                            });
                        }

                        result[propertyAsString] = list;
                    }
                    else
                    {
                        result[propertyAsString] = propertyValue == null ? "null" : propertyValue.ToString();
                    }
                }
                else
                {
                    result[propertyAsString] = propertyValue == null ? "null" : propertyValue.ToString();
                }
            }

            return result;
        }

        [Route("item")]
        public object GetItem(string ws, string extent, string item)
        {
            Workspace<IExtent> foundWorkspace;
            IUriExtent foundExtent;
            _workspaceCollection.RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

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

            AutoGenerateFormRows(foundElement, itemModel);

            return itemModel;
        }

        [NonAction]
        private void AutoGenerateFormRows(IElement foundElement, ItemContentModel itemModel)
        {
            var asAllProperties = foundElement as IObjectAllProperties;
            if (asAllProperties == null)
            {
                throw new InvalidOperationException("FoundElement is not an Instance of IObjectAllProperties");
            }

            foreach (var property in asAllProperties.getPropertiesBeingSet())
            {
                var newRow = new DataFormRow()
                {
                    name = property.ToString(),
                    title = property.ToString()
                };

                itemModel.c.Add(newRow);
            }
        }

        [Route("item_create")]
        [HttpPost]
        public object CreateItem([FromBody] ItemCreateModel model)
        {
            Workspace<IExtent> foundWorkspace;
            IUriExtent foundExtent;
            _workspaceCollection.RetrieveWorkspaceAndExtent(model.ws, model.extent, out foundWorkspace, out foundExtent);

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
                _workspaceCollection.FindItem(
                    model.AsItem(),
                    out foundWorkspace, 
                    out foundExtent, 
                    out foundItem);

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
                _workspaceCollection.FindItem(
                    model.AsItem(), 
                    out foundWorkspace, 
                    out foundExtent, 
                    out foundItem);

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
                _workspaceCollection.FindItem(
                    model.AsItem(), 
                    out foundWorkspace, 
                    out foundExtent, 
                    out foundItem);

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
                _workspaceCollection.FindItem(
                    model.AsItem(), 
                    out foundWorkspace, 
                    out foundExtent, 
                    out foundItem);

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
    }
}