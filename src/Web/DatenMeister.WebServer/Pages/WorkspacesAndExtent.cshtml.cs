﻿using System.Collections.Generic;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class WorkspacesAndExtent : PageModel
    {
        private readonly WorkspaceController _workspaceController;

        public List<WorkspaceModel> Workspaces = new();

        public WorkspacesAndExtent(WorkspaceController workspaceController)
        {
            _workspaceController = workspaceController;
        }

        public void OnGet()
        {
            Workspaces = _workspaceController.GetWorkspaceModels();
        }
    }
}