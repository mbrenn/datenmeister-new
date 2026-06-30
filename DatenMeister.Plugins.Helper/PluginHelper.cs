
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.WebServer.Library.PageRegistration;

namespace DatenMeister.Plugins.Helper;

/// <summary>
/// Implements several helper function to realize plugins.
/// It also gives an inspiration of how to implement a plugin
/// </summary>
/// <param name="configuration">The configuration which contains the main configuration and context
/// of the current plugin</param>
public class PluginHelper(PluginHelperConfiguration configuration)
{
    /// <summary>
    /// Loads an adds an extent from the manifest. It used the configuration to identify the assembly
    /// </summary>
    /// <param name="manifestPath">Relative path of the manifest without the assembly name.
    /// (e.g. 'Xmi.Types.xmi' for a file 'Xmi/Types.xmi')</param>
    /// <param name="extentName">Name of the extent in which the manifest will be added</param>
    public void AddExtentForTypesFromManifest(string manifestPath, string extentName)
    {
        if (configuration.Position != PluginLoadingPosition.AfterLoadingOfExtents) 
            return;
        
        var extentManager = new ExtentManager(configuration.WorkspaceLogic, configuration.ScopeStorage);
        var resourcePathTypes = configuration.AssemblyName + "." + manifestPath;
        extentManager.LoadNonPersistentExtentFromResources(configuration.PluginType, resourcePathTypes,
            WorkspaceNames.WorkspaceTypes, extentName);
    }

    /// <summary>
    /// Adds a javascript file to the webserver and adds it to the automatic loading
    /// </summary>
    /// <param name="parameter">Parameter of the function</param>
    public void AddJavaScriptFileToWebServer(AddJavaScriptFileToWebserverParameter parameter)
    {
        // Adds the javascript
        var pageRegistrationData = configuration.ScopeStorage.Get<PageRegistrationData>();
        var pageRegistrationLogic = new PageRegistrationLogic(pageRegistrationData);
        pageRegistrationLogic.AddJavaScriptFromResource(
            configuration.PluginType,
            configuration.AssemblyName + "." + parameter.RelativePathWithDots + "." + parameter.FileName,
            configuration.AssemblyName + "." + parameter.FileName,
            Path.Combine(
                parameter.ProjectsRelativePathToDevelopmentFile,
                parameter.DirectoryPath, 
                parameter.FileName));
    }
}