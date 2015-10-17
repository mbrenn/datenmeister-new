using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web
{
    /// <summary>
    /// Defines the core being used as the central connection point
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Gets the singleton object
        /// </summary>
        public static Core TheOne
        {
            get;
            private set;
        }

        static Core()
        {
            TheOne = new Core();
        }
        
        /// <summary>
        /// Stores the workspace for all extents
        /// </summary>
        private List<Workspace<EMOF.Interface.Identifiers.IExtent>> workspaces;

        /// <summary>
        /// Gets all the workspaces 
        /// </summary>
        public IEnumerable<Workspace<EMOF.Interface.Identifiers.IExtent>> Workspaces
        {
            get
            {
                return workspaces;
            }
        }

        public void AddWorkspace(Workspace<EMOF.Interface.Identifiers.IExtent> workspace)
        {
            // Check, if id of workspace is already known
            if (workspaces.Any(x => x.id == workspace.id))
            {
                throw new InvalidOperationException("id is already known");
            }

            workspaces.Add(workspace);
        }

        /// <summary>
        /// Initializes the information
        /// </summary>
        public void Init()
        {
            workspaces = new List<Workspace<EMOF.Interface.Identifiers.IExtent>>();
            workspaces.Add(new Workspace<EMOF.Interface.Identifiers.IExtent>("Data", "All the data workspaces"));
        }
    }
}
