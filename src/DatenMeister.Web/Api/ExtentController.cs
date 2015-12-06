using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DatenMeister.Web.Api
{
    [RoutePrefix("api/datenmeister/extent")]
    public class ExtentController : ApiController
    {
        [Route("all")]
        public object GetAll(string ws)
        {
            var result = new List<object>();
            Workspace<IExtent> workspace = GetWorkspace(ws);

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
        /// Gets the workspace by name, if workspace is not found, an exception 
        /// will be thrown
        /// </summary>
        /// <param name="ws">Name of the workspace to be queried</param>
        /// <returns>The found workspace. If the workspace is not found, an
        /// exception is thrown/returns>
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
                .Select(x => new DataTableColumn()
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
                .Select(x => new DataTableItem()
                {
                    uri = foundExtent.uri(x as IElement),
                    v = ObjectHelper.AsStringDictionary(x as IElement, properties)
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
                return null;
            }

            var foundProperties = ExtentHelper.GetProperties(foundExtent);
            foreach (var property in foundProperties)
            {
                if (foundElement.isSet(property))
                {
                    itemModel.values[property.ToString()] = foundElement.get(property).ToString();
                }
            }

            return itemModel;
        }

        /// <summary>
        /// Gets the extentmodel for a given extent
        /// </summary>
        /// <param name="ws">Workspace to be queried</param>
        /// <param name="extent">Extent to be querued</param>
        /// <returns>The found extent or an exception.</returns>
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

        public static void RetrieveWorkspaceAndExtent(string ws, string extent, out Workspace<IExtent> foundWorkspace, out IUriExtent foundExtent)
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
    }
}
