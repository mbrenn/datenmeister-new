using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Web
{
    /// <summary>
    ///     Defines the core being used as the central connection point
    /// </summary>
    public class Core
    {
        /// <summary>
        ///     Stores the workspace for all extents
        /// </summary>
        private List<Workspace<IExtent>> workspaces;

        static Core()
        {
            TheOne = new Core();
        }

        /// <summary>
        ///     Gets the singleton object
        /// </summary>
        public static Core TheOne { get; private set; }

        /// <summary>
        ///     Gets all the workspaces
        /// </summary>
        public IEnumerable<Workspace<IExtent>> Workspaces
        {
            get
            {
                lock (workspaces)
                {
                    return workspaces.ToList();
                }
            }
        }

        public void AddWorkspace(Workspace<IExtent> workspace)
        {
            lock (workspaces)
            {
                // Check, if id of workspace is already known
                if (workspaces.Any(x => x.id == workspace.id))
                {
                    throw new InvalidOperationException("id is already known");
                }

                workspaces.Add(workspace);
            }
        }

        /// <summary>
        ///     Initializes the information
        /// </summary>
        public void Init()
        {
            workspaces = new List<Workspace<IExtent>>();
            AddWorkspace(new Workspace<IExtent>("Data", "All the data workspaces"));
            Debug.WriteLine("DatenMeister Webcore initialized");
        }
    }
}