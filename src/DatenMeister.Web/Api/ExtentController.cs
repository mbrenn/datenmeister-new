using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using DatenMeister.CSV;
using DatenMeister.CSV.Runtime.Storage;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
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
        private const int MaxItemAmount = 100;

        private readonly IFactoryMapper _mapper;
        private readonly IWorkspaceCollection _workspaceCollection;
        private readonly IUmlNameResolution _resolution;
        private readonly IExtentStorageLoader _extentStorageLoader;
        private readonly IDataLayerLogic _dataLayerLogic;


        public ExtentController(
            IFactoryMapper mapper, 
            IWorkspaceCollection workspaceCollection, 
            IUmlNameResolution resolution, 
            IExtentStorageLoader extentStorageLoader, 
            IDataLayerLogic dataLayerLogic)
        {
            _mapper = mapper;
            _workspaceCollection = workspaceCollection;
            _resolution = resolution;
            _extentStorageLoader = extentStorageLoader;
            _dataLayerLogic = dataLayerLogic;
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
                        dataLayer = _dataLayerLogic.GetDataLayerOfExtent(extent).Name,
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
        /// Deletes a complete extent
        /// </summary>
        /// <param name="model">Model to be deleted</param>
        /// <returns>true, if ok</returns>
        [Route("extent_create")]
        [HttpPost]
        public object CreateExtent([FromBody] ExtentCreateModel model)
        {
            var workspace = _workspaceCollection.Workspaces.First(x => x.id == model.workspace);
            if (workspace == null)
            {
                throw new InvalidOperationException("Workspace not found");
            }

            var filename = Path.GetFileName(model.filename);
            var appBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            filename = Path.Combine(appBase, "data", filename);

            // Creates the new workspace

            IUriExtent createdExtent;
            switch (model.extentType)
            {
                default:
                    var extentData = new CSVStorageConfiguration
                    {
                        ExtentUri = model.contextUri,
                        Path = filename,
                        Workspace = model.workspace,
                        Settings = new CSVSettings()
                    };

                    extentData.Settings.Columns.AddRange(model.ColumnsAsEnumerable);

                    createdExtent = _extentStorageLoader.LoadExtent(extentData, true);
                    break;
            }

            return new
            {
                success = true,
                uri = createdExtent.contextURI()
            };
        }

        /// <summary>
        /// Deletes a complete extent
        /// </summary>
        /// <param name="model">Model to be deleted</param>
        /// <returns>true, if ok</returns>
        [Route("extent_delete")]
        [HttpPost]
        public object DeleteExtent([FromBody] ExtentReferenceModel model)
        {
            var workspace = GetWorkspace(model.ws);
            var removed = workspace.RemoveExtent(model.extent);

            return new
            {
                success = removed
            };
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
        public object GetItems(string ws, string extent, string search = null, int o = 0, int a = MaxItemAmount)
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
            var columns = columnCreator.FindColumnsForTable(foundExtent).ToList();
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
                        v = ConvertToJson(foundExtent, x as IElement, columnCreator)
                    })
                    .ToList()
            };

            return result;
        }

        private Dictionary<string, object> ConvertToJson(IUriExtent extent, IObject element, ColumnCreator creator)
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
                            var asElement = listValue as IElement;
                            string url;
                            if (asElement != null)
                            {
                                url = extent.uri(asElement);
                            }
                            else
                            {
                                url = null;
                            }

                            list.Add(new
                            {
                                u = url,
                                v = listValue == null ? "null" : _resolution.GetName(listValue)
                            });
                        }

                        result[propertyAsString] = list;
                    }
                    else
                    {
                        result[propertyAsString] = propertyValue == null ? "null" : _resolution.GetName(propertyValue);
                    }
                }
                else
                {
                    result[propertyAsString] = propertyValue == null ? "null" : _resolution.GetName(propertyValue);
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

            var itemModel = new ItemContentModel
            {
                uri = item
            };

            // Retrieves the values of the item
            var foundElement = foundExtent.element(item);
            if (foundElement == null)
            {
                // Not found
                return NotFound();
            }
            
            var columnCreator = new ColumnCreator();
            itemModel.c = columnCreator.FindColumnsForItem(foundElement).ToList();
            itemModel.v = ConvertToJson(foundExtent, foundElement, columnCreator);

            // Check, if item is of type IElement and has a metaclass
            var metaClass = foundElement.getMetaClass();
            if (metaClass != null)
            {
                var dataLayer =_dataLayerLogic.GetDataLayerOfObject(metaClass);
                var metaLayer = _dataLayerLogic.GetMetaLayerFor(dataLayer);
                var extents  = _dataLayerLogic.GetExtentsForDatalayer(metaLayer);
                var extentWithMetaClass = extents.WithElement(metaClass);

                var metaClassModel = new ItemModel
                {
                    name = _resolution.GetName(metaClass),
                    uri = extentWithMetaClass?.uri(metaClass),
                    ext = extentWithMetaClass?.contextURI(),
                    ws = _workspaceCollection.Workspaces.FindWorkspace(extentWithMetaClass)?.id
                };

                itemModel.metaclass = metaClassModel;
            }

            return itemModel;
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