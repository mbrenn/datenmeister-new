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
            var foundWorkspace = Core.TheOne.Workspaces.FirstOrDefault(x => x.id == ws);

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
                var extentModel = ExtentController.GetExtentModel(ws, extent);

                return View(extentModel);

            }
            catch (OperationFailedException exc)
            {
                return View(exc.Message);
            }
        }

        public ActionResult Item(string ws, string extent, string item)
        {
            var extentModel = ExtentController.GetExtentModel(ws, extent);
            var model = new ItemModel(extentModel, item);

            return View(model);
        }
    }
}
