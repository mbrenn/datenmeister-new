namespace DatenMeister.Plugins.Helper;

/// <summary>
/// Defines the parameter for adding a javascript file to the webserver
/// </summary>
public class AddJavaScriptFileToWebserverParameter
{
    /// <summary>
    /// Gets or set the name of the javascript file
    /// </summary>
    public string FileName { get; set; } = "";

    /// <summary>
    /// Gets or sets the relative path of the Javascript file within the project's structure and
    /// therefore of the resulting embedded file of the assembly.
    /// It shall not start with a slash.  This may be 'Js'.
    /// </summary>
    public string DirectoryPath { get; set; } = "";

    /// <summary>
    /// Gets or sets the relative path of the project which allows navigation from the resulting binary
    /// of final. It shall just refer to the project directory itself where usually the .csproj file is
    /// residing
    /// </summary>
    public string ProjectsRelativePathToDevelopmentFile { get; set; } = "";

    /// <summary>
    /// Converts the relative path into a relative path with dots
    /// </summary>
    public string RelativePathWithDots 
        => DirectoryPath.Replace('\\', '.').Replace('/', '.');

    /// <summary>
    /// Sets the FileName and RelativePathInAssembly by 
    /// </summary>
    /// <param name="pathAndFile">The path and filename. E.g 'js/logic.js'</param>
    public void SetByRelativeFileName(string pathAndFile)
    {
        FileName = Path.GetFileName(pathAndFile);
        DirectoryPath = Path.GetDirectoryName(pathAndFile) ?? "";
    }
}