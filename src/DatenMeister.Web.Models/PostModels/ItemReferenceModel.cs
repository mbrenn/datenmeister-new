using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Web.Models.PostModels
{
    /// <summary>
    ///     This class is used to reference a single object within the database
    /// </summary>
    public class ItemReferenceModel : ExtentReferenceModel
    {
        public string item { get; set; }

        public WorkspaceExtentAndItemReference AsItem()
        {
            return new WorkspaceExtentAndItemReference(ws, extent, item);
        }
    }
}