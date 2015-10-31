using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                var workspace = new Workspace<IExtent>("ID");
                var model = new WorkspaceModel(foundWorkspace);

                return View(model);
            }
        }

        public ActionResult Extent(string ws, string extent)
        {
            var foundWorkspace = Core.TheOne.Workspaces.Where(x => x.id == ws).FirstOrDefault();

            if (foundWorkspace == null)
            {
                return View("Workspace_NotFound");
            }

            var foundExtent = foundWorkspace.extent.Cast<IUriExtent>().Where(x => x.contextURI() == extent).FirstOrDefault();
            if (foundExtent == null)
            {
                return View("Extent_NotFound");
            }

            var model = new ExtentModel(
                foundExtent,
                new WorkspaceModel(foundWorkspace));

            return View(model);
        }
    }
}
