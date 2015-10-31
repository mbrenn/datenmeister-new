using DatenMeister.EMOF.Interface.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web.Models
{
    /// <summary>
    /// Stores the data for an extent
    /// </summary>
    public class ExtentModel
    {
        public string url
        {
            get;
            set;
        }

        public WorkspaceModel workspace
        {
            get;
            set;
        }

        public ExtentModel(IUriExtent extent, WorkspaceModel workspace)
        {
            this.url = extent.contextURI();
            this.workspace = workspace;
        }
    }
}
