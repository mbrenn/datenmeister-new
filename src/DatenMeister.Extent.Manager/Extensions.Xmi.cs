using System.Reflection;
using System.Xml.Linq;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Extent.Manager;

public static class XmiExtensions
{
    /// <summary>
    /// Creates a new xmi extent and adds it to the
    /// </summary>
    /// <param name="uri">Uri being used</param>
    /// <returns>The created xmi extent</returns>
    public static IUriExtent CreateXmiExtent(string uri, IScopeStorage? scopeStorage = null)
    {
        var xmlProvider = new XmiProvider();
        return new MofUriExtent(xmlProvider, uri, scopeStorage);
    }

    /// <summary>
    /// CReates the
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="uri"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static async Task<ExtentStorageData.LoadedExtentInformation> CreateAndAddXmiExtent(
        ExtentManager scope,
        string uri,
        string filename)
    {
        return await scope.CreateAndAddXmiExtent(uri, filename);
    }

    /// <summary>
    /// Creates an extent by loading a resource from an an assembly
    /// </summary>
    /// <param name="extentManager">Extent Manager to be used</param>
    /// <param name="manifestType">Type defining the assembly from which the resource will be loaded</param>
    /// <param name="manifestName">Name of the manifest</param>
    /// <param name="uri">Uri of the extent to which the element will be loaded</param>
    /// <param name="workspace">Workspace to which the extent will be loaded</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Exception in case of a failure</exception>
    public static IUriExtent CreateExtentByResource(
        this ExtentManager extentManager,
        Type manifestType, 
        string manifestName,
        string uri,
        string workspace)
    {
        using var stream = manifestType.GetTypeInfo()
            .Assembly.GetManifestResourceStream(manifestName);

        if (stream == null)
        {
            throw new InvalidOperationException($"The stream for {manifestName} could not be opened");
        }

        var document = XDocument.Load(stream);
        return extentManager.CreateXmiExtentByDocument(uri, document, workspace);
    }

    /// <summary>
    /// Loads types and creates a management extent from the specified embedded resource.
    /// </summary>
    /// <param name="extentManager">The manager responsible for handling extents within the application.</param>
    /// <param name="sourceAssemblyType">The type located in the assembly containing the embedded resource.</param>
    /// <param name="sourceResourcePath">The path to the embedded resource within the assembly.</param>
    /// <param name="targetWorkspace">The name of the target workspace to which the extent will be added.</param>
    /// <param name="targetExtentUri">The URI of the extent to be created.</param>
    /// <returns>The created URI-based extent.</returns>
    public static IUriExtent LoadNonPersistentExtentFromResources(
        this ExtentManager extentManager,
        Type sourceAssemblyType,
        string sourceResourcePath,
        string targetWorkspace,
        string targetExtentUri)
    {
        // Loads the Documents
        var xmiText = ResourceHelper.LoadStringFromAssembly(sourceAssemblyType, sourceResourcePath);
                
        // Loads the XDocument from the loaded Resource Document, reflecting the content. 
        var xmiDocument = XDocument.Parse(xmiText);

        // Loads the providers
        var xmiProvider = new XmiProvider(xmiDocument);
                
        // Now, loads the UriExtents
        var xmiExtent = new MofUriExtent(xmiProvider, targetExtentUri, extentManager.ScopeStorage);
                
        // Adds the extents to the workspaces directly, so they won't be loaded by the ExtentManager on Application Start-Up
        extentManager.AddNonPersistentExtent(targetWorkspace, xmiExtent);

        return xmiExtent;
    }
}