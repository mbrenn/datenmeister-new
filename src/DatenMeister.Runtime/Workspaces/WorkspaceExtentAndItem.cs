namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    ///     This class is used to reference a single object within the database
    /// </summary>
    public class WorkspaceExtentAndItem
    {
        public string ws { get; }
        public string extent { get; }
        public string item { get; }

        public WorkspaceExtentAndItem(string ws, string extent, string item)
        {
            this.ws = ws;
            this.extent = extent;
            this.item = item;
        }
    }
}