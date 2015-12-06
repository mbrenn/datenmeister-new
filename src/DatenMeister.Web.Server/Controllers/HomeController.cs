using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Web.Models;
using System.Linq;
using System.Web.Mvc;
using DatenMeister.Web.Api;

namespace DatenMeister.Web.Server.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Workspace(string ws)
        {
            var foundWorkspace = Core.TheOne.Workspaces.Where(x => x.id == ws).FirstOrDefault();

            if (foundWorkspace == null)
            {
                return View("Workspace_NotFound");
            }
            else
            {
                var model = new WorkspaceModel(foundWorkspace);

                return View(model);
            }
        }

        public ActionResult Extent(string ws, string extent)
        {
            try
            {
                var extentModel = GetExtentModel(ws, extent);

                return View(extentModel);

            }
            catch (OperationFailedException exc)
            {
                return View(exc.Message);
            }
        }

        public ActionResult Item(string ws, string extent, string item)
        {
            var extentModel = GetExtentModel(ws, extent);
            var model = new ItemModel(extentModel, item);

            return View(model);
        }

        /// <summary>
        /// Gets the extentmodel for a given extent
        /// </summary>
        /// <param name="ws">Workspace to be queried</param>
        /// <param name="extent">Extent to be querued</param>
        /// <returns>The found extent or an exception.</returns>
        private static ExtentModel GetExtentModel(string ws, string extent)
        {
            var foundWorkspace = Core.TheOne.Workspaces.FirstOrDefault(x => x.id == ws);

            if (foundWorkspace == null)
            {
                throw new OperationFailedException("Workspace_NotFound");
            }

            var foundExtent = foundWorkspace.extent.Cast<IUriExtent>().FirstOrDefault(x => x.contextURI() == extent);
            if (foundExtent == null)
            {
                throw new OperationFailedException("Extent_NotFound");
            }

            var extentModel = new ExtentModel(
                foundExtent,
                new WorkspaceModel(foundWorkspace));
            return extentModel;
        }
    }
}
