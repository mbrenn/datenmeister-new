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
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.Web.Helper;
using DatenMeister.Web.Models;
using DatenMeister.Web.Models.PostModels;
using DatenMeister.XMI.ExtentStorage;

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
        private readonly ColumnCreator _columnCreator;
        private readonly ExtentFunctions _extentFunctions;


        public ExtentController(
            IFactoryMapper mapper, 
            IWorkspaceCollection workspaceCollection, 
            IUmlNameResolution resolution, 
            IExtentStorageLoader extentStorageLoader, 
            IDataLayerLogic dataLayerLogic,
            ColumnCreator columnCreator, 
            ExtentFunctions extentFunctions)
        {
            _mapper = mapper;
            _workspaceCollection = workspaceCollection;
            _resolution = resolution;
            _extentStorageLoader = extentStorageLoader;
            _dataLayerLogic = dataLayerLogic;
            _columnCreator = columnCreator;
            _extentFunctions = extentFunctions;
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


        private static string RemoveQuotesFromFilename(string filename)
        {
            // Remove quotes, if filename is fully quoted
            if (filename.StartsWith("\"") && filename.EndsWith("\"") && filename.Length >= 2)
            {
                filename = filename.Substring(1, filename.Length - 2);
            }
            return filename;
        }
        
        /// <summary>
        /// Deletes a complete extent
        /// </summary>
        /// <param name="model">Model to be deleted</param>
        /// <returns>true, if ok</returns>
        [Route("extent_add")]
        [HttpPost]
        public object AddExtent([FromBody] ExtentAddModel model)
        {
            var workspace = _workspaceCollection.Workspaces.First(x => x.id == model.workspace);
            if (workspace == null)
            {
                throw new InvalidOperationException("Workspace not found");
            }
            
            var filename = RemoveQuotesFromFilename(model.filename);
            filename = MakePathAbsolute(filename);

            // Creates the new workspace
            var configuration = GetStorageConfiguration(model, filename);
            var createdExtent = _extentStorageLoader.LoadExtent(configuration, true);

            return new
            {
                success = true,
                uri = createdExtent.contextURI()
            };
        }

        private static string MakePathAbsolute(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(filename));
            }

            if (!Path.IsPathRooted(filename))
            {
                filename = Path.GetFileName(filename);
                var appBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                filename = Path.Combine(appBase, "App_Data/Database", filename);
            }
            return filename;
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

            var filename = MakePathAbsolute(model.filename);

            // Creates the new workspace
            var configuration = GetStorageConfiguration(model, filename);
            var createdExtent = _extentStorageLoader.LoadExtent(configuration, true);

            return new
            {
                success = true,
                uri = createdExtent.contextURI()
            };
        }

        /// <summary>
        /// Gets the configuration by using the given model and their filename
        /// </summary>
        /// <param name="model">Model to be used to retrieve the information</param>
        /// <param name="filename">Filename to be used</param>
        /// <returns></returns>
        private static ExtentStorageConfiguration GetStorageConfiguration(ExtentAddModel model, string filename)
        {
            ExtentStorageConfiguration configuration;
            switch (model.type)
            {
                case "xmi":
                    configuration = new XmiStorageConfiguration
                    {
                        ExtentUri = model.contextUri,
                        Path = filename,
                        Workspace = model.workspace
                    };

                    break;
                case "csv":
                    var csvExtentData = new CSVStorageConfiguration
                    {
                        ExtentUri = model.contextUri,
                        Path = filename,
                        Workspace = model.workspace,
                        Settings = new CSVSettings()
                    };

                    var modelAsCreateModel = model as ExtentCreateModel;
                    if (modelAsCreateModel != null)
                    {
                        foreach (var c in modelAsCreateModel.ColumnsAsEnumerable)
                        {
                            csvExtentData.Settings.Columns.Add(c);
                        }
                    }

                    configuration = csvExtentData;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown extent type: {model.type}");
            }
            return configuration;
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
        /// Gets an enumeration of creatable types for the given extent.
        /// The user can create a new instance by using this list
        /// </summary>
        /// <param name="ws">Workspace being used</param>
        /// <param name="extent">Extent being used</param>
        /// <returns>Array of creatable types</returns>
        [Route("get_creatable_types")]
        [HttpGet]
        public object GetCreatableTypes(string ws, string extent)
        {
            Workspace<IExtent> foundWorkspace;
            IUriExtent foundExtent;
            _workspaceCollection.RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

            var foundTypes = _extentFunctions.GetCreatableTypes(foundExtent);

            return new
            {
                types = from type in foundTypes.CreatableTypes
                    let typeExtent = type.GetUriExtentOf()
                    select new
                    {
                        name = _resolution.GetName(type),
                        uri = typeExtent.uri(type),
                        ext = typeExtent.contextURI(),
                        ws = _workspaceCollection.FindWorkspace(typeExtent)
                    }
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


            var result = _columnCreator.FindColumnsForTable(foundExtent);
            var properties = result.Properties;

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
            var resultModel = new ExtentContentModel
            {
                url = extent,
                columns = result.Columns,
                totalItemCount = totalItems.Count(),
                search = search,
                filteredItemCount = filteredAmount,
                items = filteredItems
                    .Select(x => new DataTableItem
                    {
                        uri = foundExtent.uri(x as IElement),
                        v = ConvertToJson(x as IElement, result)
                    })
                    .ToList()
            };

            return resultModel;
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

            var result = _columnCreator.FindColumnsForItem(foundElement);
            itemModel.c = result.Columns.ToList();
            itemModel.v = ConvertToJson(foundElement, result);
            itemModel.layer = _dataLayerLogic?.GetDataLayerOfObject(foundElement)?.Name;

            // Check, if item is of type IElement and has a metaclass
            var metaClass = foundElement.getMetaClass();
            if (metaClass != null)
            {
                var dataLayer =_dataLayerLogic?.GetDataLayerOfObject(metaClass);
                var extents  = _dataLayerLogic?.GetExtentsForDatalayer(dataLayer);
                var extentWithMetaClass = extents.WithElement(metaClass);

                var metaClassModel = new ItemModel
                {
                    name = _resolution.GetName(metaClass),
                    uri = extentWithMetaClass?.uri(metaClass),
                    ext = extentWithMetaClass?.contextURI(),
                    ws = _workspaceCollection.Workspaces.FindWorkspace(extentWithMetaClass)?.id,
                    layer = dataLayer?.Name
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

            // If the metaclass was given, look for it, otherwise we do not have a type
            IElement metaclass = null;
            if (!string.IsNullOrEmpty(model.metaclass))
            {
                metaclass = _workspaceCollection.FindItem(model.metaclass);
            }

            // Creates the type
            var factory = _mapper.FindFactoryFor(foundExtent);
            var element = factory.create(metaclass);

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

                var property = _columnCreator.ConvertColumnNameToProperty(model.property);
                foundItem.unset(property);

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

                var property = _columnCreator.ConvertColumnNameToProperty(model.property);
                foundItem.set(property, model.newValue);

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

                if (model.v != null)
                {
                    foreach (var pair in model.v)
                    {
                        var property = _columnCreator.ConvertColumnNameToProperty(pair.Key);
                        foundItem.set(property, pair.Value);
                    }
                }

                return new {success = true};
            }
            catch (OperationFailedException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Converts a given element to a json string, dependent on the column definition as given by the 
        /// ColumnCreationResult
        /// </summary>
        /// <param name="element"></param>
        /// <param name="creatorResult"></param>
        /// <returns></returns>
        private Dictionary<string, object> ConvertToJson(IObject element, ColumnCreationResult creatorResult)
        {
            var result = new Dictionary<string, object>();

            foreach (var property in creatorResult.Properties
                .Where(property => element.isSet(property)))
            {
                var propertyAsString = ColumnCreator.ConvertPropertyToColumnName(property);

                var propertyValue = element.get(property);

                if (creatorResult.ColumnsOnProperty[property].isEnumeration)
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
                                url = asElement.GetUri();
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

    }
}