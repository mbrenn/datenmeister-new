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

        public ActionResult Workspace(string id)
        {
            var foundWorkspace = Core.TheOne.Workspaces.Where(x => x.id == id).FirstOrDefault();

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
    }
}
