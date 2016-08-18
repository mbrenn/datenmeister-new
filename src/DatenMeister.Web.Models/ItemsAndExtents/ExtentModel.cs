using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Models.ItemsAndExtents
{
    /// <summary>
    ///     Stores the data for an extent
    /// </summary>
    public class ExtentModel
    {
        public ExtentModel(IUriExtent extent, WorkspaceModel workspace)
        {
            url = extent.contextURI();
            this.workspace = workspace;
        }

        public string url { get; set; }

        public WorkspaceModel workspace { get; set; }
    }
}