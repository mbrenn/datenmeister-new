using System.Linq;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

namespace DatenMeister.WebServer.Library.ServerConfiguration
{
    /// <summary>
    /// The class which supports the loading of the server settings
    /// </summary>
    public class SettingsLoader
    {
        /// <summary>
        /// Loads the setting 
        /// </summary>
        /// <param name="extent">Extent being queried</param>
        /// <returns>The found serversettings</returns>
        public static ServerSettings LoadSettingsFromExtent(IExtent extent)
        {
            // Gets the first element
            var foundElement = extent.elements().OfType<IElement>().FirstOrDefault();
            if (foundElement == null)
            {
                return new ServerSettings();
            }
            
            // Gets the web property
            var webElement = foundElement.getOrDefault<IElement>("web");
            if (webElement == null)
            {
                return new ServerSettings();
            }

            return DotNetConverter.ConvertToDotNetObject<ServerSettings>(webElement);
        }
    }
}