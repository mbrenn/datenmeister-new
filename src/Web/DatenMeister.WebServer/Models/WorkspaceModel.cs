namespace DatenMeister.WebServer.Models;

public class WorkspaceModel
{
    public string id { get; set; } = string.Empty;
    public string annotation { get; set; } = string.Empty;

    public List<ExtentModel> extents { get; } = new();
}