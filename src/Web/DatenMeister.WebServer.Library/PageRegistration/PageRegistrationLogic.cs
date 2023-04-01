using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DatenMeister.WebServer.Library.PageRegistration
{
    public class PageRegistrationLogic
    {
        private readonly PageRegistrationData _data;

        /// <summary>
        /// Initializes a new instance of the page registration
        /// </summary>
        /// <param name="data">Data</param>
        public PageRegistrationLogic(PageRegistrationData data)
        {
            _data = data;
        }

        /// <summary>
        /// Adds a special url to the page factory
        /// </summary>
        /// <param name="url">Url to be added</param>
        /// <param name="contentType">The MIME-Cotnent</param>
        /// <param name="pageStreamFactory">Page Stream Factory to be used</param>
        public void AddUrl(string url, string contentType, Func<Stream> pageStreamFactory)
        {
            _data.PageFactories.Add(
                new PageFactory(url, contentType, pageStreamFactory));
        }

        /// <summary>
        /// Adds a new Javascript file for the webserver
        /// The JavaScript file is taken from a resource and will be available under js/{fileName}
        /// </summary>
        /// <param name="manifestType">Type in which the manifest is stored</param>
        /// <param name="manifestName">Name of the manifest</param>
        /// <param name="fileName">Name of the file</param>
        public void AddJavaScriptFromResource(
            Type manifestType,
            string manifestName,
            string fileName)
        {
            AddUrl(
                $"js/{fileName}",
                "application/javascript",
                () =>
                {
                    var result = manifestType.GetTypeInfo()
                        .Assembly.GetManifestResourceStream(manifestName);
                    return result ?? throw new InvalidOperationException($"The manifest {manifestName} was not found");
                });
            
            _data.JavaScriptFiles.Add(fileName);
        }

        /// <summary>
        /// Finds a special page factory by the url
        /// </summary>
        /// <param name="url">Url to be evaluated</param>
        /// <returns>The found pagefactory</returns>
        public PageFactory? FindPageFactoryByUrl(string url)
        {
            return _data.PageFactories.FirstOrDefault(x => x.Url == url);
        }
    }
}