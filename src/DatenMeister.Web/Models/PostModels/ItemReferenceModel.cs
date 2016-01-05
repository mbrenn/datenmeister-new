using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Web.Models.PostModels
{
    /// <summary>
    ///     This class is used to reference a single object within the database
    /// </summary>
    public class ItemReferenceModel : ExtentReferenceModel
    {
        public string item { get; set; }

        public WorkspaceExtentAndItem AsItem()
        {
            return new WorkspaceExtentAndItem(ws, extent, item);
        }
    }
}