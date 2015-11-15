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
            var workspace = Core.TheOne.Workspaces.Where(x => x.id == ws).First();
            if (workspace == null)
            {
                throw new InvalidOperationException("Workspace not found");
            }

            return workspace;
        }

        [Route("items")]
        public object GetItems(string ws, string url)
        {
            var amount = 100; // Return only the first 100 elements if no index is given
            var offset = 0;
            var workspace = GetWorkspace(ws);
            var foundExtent =
                workspace.extent
                    .Cast<IUriExtent>()
                    .Where(x => x.contextURI() == url)
                    .FirstOrDefault();
            if (foundExtent == null)
            {
                throw new InvalidOperationException("Not found");
            }

            var totalItems = foundExtent.elements();
            var foundItems = totalItems;

            var properties = ExtentHelper.GetProperties(foundExtent).ToList();

            var result = new ExtentContentModel();
            result.url = url;
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
                    url = foundExtent.uri(x as IElement),
                    v = ObjectHelper.AsStringDictionary(x as IElement, properties)
                })
                .ToList();

            return result;
        }
    }
}
