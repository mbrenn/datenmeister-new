using System.Reflection;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
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
    public static IUriExtent CreateXmiExtent(string uri)
    {
        var xmlProvider = new XmiProvider();
        return new MofUriExtent(xmlProvider, uri, null);
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
}