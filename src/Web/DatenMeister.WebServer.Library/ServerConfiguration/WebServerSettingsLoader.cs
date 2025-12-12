using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.WebServer.Library.ServerConfiguration;

/// <summary>
/// The class which supports the loading of the server settings
/// </summary>
public static class WebServerSettingsLoader
{
    /// <summary>
    /// Loads the setting 
    /// </summary>
    /// <param name="extent">Extent being queried</param>
    /// <returns>The found serversettings</returns>
    public static WebServerSettings LoadSettingsFromExtent(IExtent extent)
    {
        // Gets the first element
        var foundElement = extent.elements().OfType<IElement>().FirstOrDefault();
        if (foundElement == null)
        {
            return new WebServerSettings();
        }
            
        // Gets the web property
        var webElement = foundElement.getOrDefault<IElement>("web");
        if (webElement == null)
        {
            return new WebServerSettings();
        }

        return DotNetConverter.ConvertToDotNetObject<WebServerSettings>(webElement);
    }
}