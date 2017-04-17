﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Models.ItemsAndExtents;
using DatenMeister.Models.PostModels;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.CSV.Runtime.Storage;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Dynamic;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DatenMeister.Web.Api
{
    [Route("api/datenmeister/extent")]
    public class ExtentController : Controller
    {
        /// <summary>
        /// Defines the maximum numnber of items that shall be returned via GetItems
        /// </summary>
        private const int MaxItemAmount = 100;
        
        private readonly IUmlNameResolution _resolution;
        private readonly IExtentStorageLoader _extentStorageLoader;
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentFunctions _extentFunctions;
        private readonly ILifetimeScope _diScope;
        private readonly IViewFinder _viewFinder;
        private readonly NamedElementMethods _namedElementMethods;

        public ExtentController(
            IUmlNameResolution resolution, 
            IExtentStorageLoader extentStorageLoader, 
            IWorkspaceLogic workspaceLogic, 
            ExtentFunctions extentFunctions,
            ILifetimeScope diScope,
            IViewFinder viewFinder,
            NamedElementMethods namedElementMethods)
        {
            _resolution = resolution;
            _extentStorageLoader = extentStorageLoader;
            _workspaceLogic = workspaceLogic;
            _extentFunctions = extentFunctions;
            _diScope = diScope;
            _viewFinder = viewFinder;
            _namedElementMethods = namedElementMethods;
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
                        dataLayer = _workspaceLogic.GetWorkspaceOfExtent(extent).id,
                        count = extent.elements().Count(),
                        type = extent.GetType().Name
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
        private Workspace GetWorkspace(string ws)
        {
            var workspace = _workspaceLogic.Workspaces.First(x => x.id == ws);
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
            var workspace = _workspaceLogic.Workspaces.First(x => x.id == model.workspace);
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
                var appBase = AppContext.BaseDirectory;
                filename = Path.Combine(appBase, "App_Data/Database", filename);
            }
            return filename;
        }

        /// <summary>
        /// Removes all invalid characters within a filename to ensure safety
        /// by disallowing navigation within the filesystem of the server
        /// </summary>
        /// <param name="filename">Filename to be strapped out</param>
        /// <returns>The cleaned filename</returns>
        private static string CleanFilenameFromInvalidCharacters(string filename)
        {
            foreach (var letter in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(letter, '_');
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
            var workspace = _workspaceLogic.Workspaces.First(x => x.id == model.workspace);
            if (workspace == null)
            {
                throw new InvalidOperationException("Workspace not found");
            }

            string filename;
            if (!string.IsNullOrEmpty(model.filename))
            {
                filename = MakePathAbsolute(model.filename);
            }
            else
            {
                filename = MakePathAbsolute(
                    Path.ChangeExtension(
                        CleanFilenameFromInvalidCharacters(model.name),
                        CleanFilenameFromInvalidCharacters(model.type)));
            }

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
            var removed = workspace.RemoveExtent(model.ext);

            return new
            {
                success = removed
            };
        }

        [Route("extent_export_csv")]
        [HttpGet]
        public object ExportExtentAsCsv(string ws, string extent)
        {
            Workspace foundWorkspace;
            IUriExtent foundExtent;
            _workspaceLogic.RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

            var provider = new CSVDataProvider(_workspaceLogic);
            
            using (var stream = new StringWriter())
            {
                provider.SaveToStream(stream, ((MofUriExtent) foundExtent).Provider, new CSVSettings());
                var httpResponse = new HttpResponseMessage
                {
                    Content = new StringContent(stream.GetStringBuilder().ToString())
                };

                var extentName = foundExtent.contextURI();
                foreach (var c in Path.GetInvalidFileNameChars())
                {
                    extentName = extentName.Replace(c, '-');
                }

                extentName = $"{extentName.Replace('=', '-')}.csv"; 

                httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                httpResponse.Content.Headers.Add("Content-Disposition", $"attachment;filename={extentName}");

                return httpResponse;
            }
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
            Workspace foundWorkspace;
            IUriExtent foundExtent;
            _workspaceLogic.RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

            var foundTypes = _extentFunctions.GetCreatableTypes(foundExtent).CreatableTypes.OrderBy(x=>x.get("name").ToString());

            return new
            {
                types = from type in foundTypes
                    let typeExtent = type.GetUriExtentOf()
                    select new
                    {
                        name = _resolution.GetName(type),
                        uri = typeExtent.uri(type),
                        ext = typeExtent.contextURI(),
                        ws = _workspaceLogic.FindWorkspace(typeExtent)?.id
                    }
            };
        }

        /// <summary>
        /// Gets all allowed views for the given extent or item. 
        /// If the item is null, then the views for the given extent will be returned
        /// </summary>
        /// <param name="ws">Workspace to be used</param>
        /// <param name="extent">Extent to be used</param>
        /// <param name="itemUrl"></param>
        [Route("get_views")]
        [HttpGet]
        public object GetViews(string ws, string extent, string itemUrl = null)
        {
            Workspace foundWorkspace;
            IUriExtent foundExtent;
            IElement foundItem = null;
            if (itemUrl != null)
            {
                _workspaceLogic.FindItem(
                    new WorkspaceExtentAndItemReference(ws, extent, itemUrl),
                    out foundWorkspace,
                    out foundExtent,
                    out foundItem);
            }
            else
            {
                _workspaceLogic.RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);
            }

            var foundViews = _viewFinder.FindViews(foundExtent, foundItem);

            return new
            {
                views = from viewType in foundViews
                    let typeExtent = viewType.GetUriExtentOf()
                    select new
                    {
                        name = _resolution.GetName(viewType),
                        uri = typeExtent.uri(viewType),
                        ext = typeExtent.contextURI(),
                        ws = _workspaceLogic.FindWorkspace(typeExtent)?.id
                    }
            };
        }

        /// <summary>
        /// Returns a list of items being in the query. 
        /// The query contains a filter and a subset of elements
        /// </summary>
        /// <param name="ws">Workspace to be queried</param>
        /// <param name="extent">Extent to be queried</param>
        /// <param name="view">Defines the name of the view to be ued</param>
        /// <param name="search">The searchtext being used for query</param>
        /// <param name="o">Offset, defining the index of the first element within the response queue</param>
        /// <param name="a">Number of items being shown</param>
        /// <returns>Enumeration of items</returns>
        [Route("items")]
        public object GetItems(string ws, string extent, string view = null, string search = null, int o = 0, int a = MaxItemAmount)
        {
            var amount = Math.Max(0, Math.Min(MaxItemAmount, a)); // Return only the first 100 elements if no index is given
            var workspace = GetWorkspace(ws);
            var foundExtent =
                workspace.extent
                    .Cast<IUriExtent>()
                    .FirstOrDefault(x => x.contextURI() == extent);

            if (foundExtent == null)
            {
                return Content(HttpStatusCode.NotFound.ToString(), "Extent Not Found");
            }

            var totalItems = foundExtent.elements();
            var foundItems = totalItems;
            
            var result = _viewFinder.FindView(foundExtent, view);
            if (result == null)
            {
                return Content(HttpStatusCode.NotFound.ToString(), "View Not Found");
            }

            var fields =
                result.GetAsReflectiveCollection(
                    _FormAndFields._Form.fields);
            var properties = fields
                .Select(x => x.AsIObject().get("name").ToString())
                .ToList();

            // Perform the filtering
            IEnumerable<object> filteredItems = foundItems;
            var metaClasses = filteredItems.GetMetaClasses().Select(x => new ItemModel
            {
                name = _resolution.GetName(x),
                uri = x.GetUri()
            });

            if (!string.IsNullOrEmpty(search))
            {
                filteredItems = foundItems.WhenOneOfThePropertyContains(properties, search, StringComparison.CurrentCultureIgnoreCase);
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
                .Take(amount)
                .ToList();

            // Now return our stuff
            var resultModel = new ExtentContentModel
            {
                url = extent,
                columns = DynamicConverter.ToDynamic(result, false),
                totalItemCount = totalItems.Count(),
                search = search,
                filteredItemCount = filteredAmount,
                metaClasses = metaClasses,
                items = filteredItems
                    .Select(x => new ItemContentModel
                    {
                        uri = foundExtent.uri(x as IElement),
                        v = ConvertToJson(x as IElement, result)
                    })
                    .ToList()
            };

            return resultModel;
        }

        [Route("item")]
        public object GetItem(string ws, string extent, string item, string view = null)
        {
            Workspace foundWorkspace;
            IUriExtent foundExtent;
            _workspaceLogic.RetrieveWorkspaceAndExtent(ws, extent, out foundWorkspace, out foundExtent);

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

            var result = _viewFinder.FindView(foundElement, view);
            itemModel.c = DynamicConverter.ToDynamic(result, false);
            itemModel.v = ConvertToJson(foundElement, result);
            itemModel.layer = _workspaceLogic?.GetWorkspaceOfObject(foundElement)?.id;

            // Check, if item is of type IElement and has a metaclass
            var metaClass = foundElement.getMetaClass();
            if (metaClass != null)
            {
                var dataLayer =_workspaceLogic?.GetWorkspaceOfObject(metaClass);
                var extents  = _workspaceLogic?.GetExtentsForWorkspace(dataLayer);
                var extentWithMetaClass = extents.WithElement(metaClass);

                var metaClassModel = new ItemModel
                {
                    name = _resolution.GetName(metaClass),
                    fullname = _namedElementMethods.GetFullName(metaClass),
                    uri = extentWithMetaClass?.uri(metaClass),
                    ext = extentWithMetaClass?.contextURI(),
                    ws = _workspaceLogic.Workspaces.FindWorkspace(extentWithMetaClass)?.id,
                    layer = dataLayer?.id
                };

                itemModel.metaclass = metaClassModel;
            }

            return itemModel;
        }

        [Route("item_create")]
        [HttpPost]
        public object CreateItem([FromBody] ItemCreateModel model)
        {
            Workspace foundWorkspace;
            IUriExtent foundExtent;
            _workspaceLogic.RetrieveWorkspaceAndExtent(model.ws, model.ext, out foundWorkspace, out foundExtent);

            /*if (!string.IsNullOrEmpty(model.container))
            {
                throw new InvalidOperationException("Element creation within container is not supported.");
            }*/

            // If the metaclass was given, look for it, otherwise we do not have a type
            IElement metaclass = null;
            if (!string.IsNullOrEmpty(model.metaclass))
            {
                metaclass = _workspaceLogic.FindItem(model.metaclass);
            }

            // Creates the type
            var factory = new MofFactory(foundExtent);
            var element = factory.create(metaclass);

            if (string.IsNullOrEmpty(model.parentItem))
            {
                // The element will be created at root of the extent
                foundExtent.elements().add(element);
            }
            else
            {
                if (string.IsNullOrEmpty(model.parentProperty))
                {
                    throw new InvalidOperationException($"No property being set for item {model.parentItem}");
                }

                // The element will be created as a subitem for the parent item
                var parentElement = _workspaceLogic.FindItem(model.parentItem);
                if (parentElement == null)
                {
                    throw new InvalidOperationException($"The parent item {model.parentItem} cannot be found");
                }

                var propertyValue = parentElement.get(model.parentProperty);
                if (propertyValue is IReflectiveCollection)
                {
                    var propertyValueAsReflectiveCollection = propertyValue as IReflectiveCollection;
                    propertyValueAsReflectiveCollection.add(element);
                }
                else
                {
                    var list = new List<object> {element};
                    parentElement.set(model.parentProperty, list);
                }
            }

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
                Workspace foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                _workspaceLogic.FindItem(
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
                Workspace foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                _workspaceLogic.FindItem(
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
                Workspace foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                _workspaceLogic.FindItem(
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
        public object SetPropertyValues([FromBody] ItemSetPropertiesModel model)
        {
            try
            {
                Workspace foundWorkspace;
                IUriExtent foundExtent;
                IElement foundItem;
                _workspaceLogic.FindItem(
                    model.AsItem(), 
                    out foundWorkspace, 
                    out foundExtent, 
                    out foundItem);

                if (model.v != null)
                {
                    foreach (var pair in model.v)
                    {
                        foundItem.set(pair.Key, pair.Value);
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
        /// <param name="element">The element that shall be converted to a json object</param>
        /// <param name="form">The form being used for conversion</param>
        /// <returns></returns>
        private Dictionary<string, object> ConvertToJson(IObject element, IObject form)
        {
            var result = new Dictionary<string, object>();

            // Goes through the fieldss of the property
            foreach (var field in form.GetAsReflectiveCollection(_FormAndFields._Form.fields)
                .Select(x=>x.AsIElement())
                .Where(field => element.isSet(field.get(_FormAndFields._Form.name).ToString())))
            {
                var property = field.get(_FormAndFields._FieldData.name).ToString();
                var propertyValue = element.get(property);

                if (ObjectHelper.IsTrue(field.get(_FormAndFields._FieldData.isEnumeration)))
                {
                    // Checks if the given element ís an enumeration
                    if (DotNetHelper.IsOfEnumeration(propertyValue))
                    {
                        var list = new List<object>();
                        foreach (var listValue in (IEnumerable) propertyValue)
                        {
                            var asElement = listValue as IElement;

                            list.Add(new
                            {
                                u = asElement?.GetUri(),
                                v = listValue == null ? "null" : _resolution.GetName(listValue)
                            });
                        }

                        result[property] = list;
                    }
                    else
                    {
                        result[property] = ConvertToJson(propertyValue);
                    }
                }
                else
                {
                    result[property] = ConvertToJson(propertyValue);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the value to a json object that can be returned to the client
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private object ConvertToJson(object propertyValue)
        {
            object tempResult;
            var asElement = propertyValue as IElement;
            if (asElement != null)
            {
                tempResult = new
                {
                    u = asElement.GetUri(),
                    v = _resolution.GetName(asElement)
                };
            }
            else
            {
                tempResult = propertyValue == null ? "null" : _resolution.GetName(propertyValue);
            }
            return tempResult;
        }
    }
}