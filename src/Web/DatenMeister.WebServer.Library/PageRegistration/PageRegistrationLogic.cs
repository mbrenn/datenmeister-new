using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DatenMeister.WebServer.Library.PageRegistration
{
    /// <summary>
    /// Defines the different registration types.
    /// OnlyEmbedding just allows the access by the url.
    /// EmbedAndLoad will trigger the webserver in a way that
    /// the css file is directly embedded into every webpage being delivered by the server
    /// </summary>
    public enum RegistrationType
    {
        /// <summary>
        /// The Javascript file will just be embedded and not loaded during the
        /// initialization of the page
        /// </summary>
        OnlyEmbedding,
        
        /// <summary>
        /// The javascript file will be embedded and loaded by each page
        /// </summary>
        EmbedAndLoad
    }
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
        /// <param name="registrationType">Type of the registration</param>
        public void AddJavaScriptFromResource(
            Type manifestType,
            string manifestName,
            string fileName,
            RegistrationType registrationType = RegistrationType.EmbedAndLoad)
        {
            AddUrl(
                $"js/datenmeister/module/{fileName}",
                "application/javascript",
                () =>
                {
                    var result = manifestType.GetTypeInfo()
                        .Assembly.GetManifestResourceStream(manifestName);
                    return result ?? throw new InvalidOperationException($"The manifest {manifestName} was not found");
                });

            if (registrationType == RegistrationType.EmbedAndLoad)
            {
                _data.JavaScriptFiles.Add(fileName);
            }
        }

        /// <summary>
        /// Adds a certain CSS File to the Webserver. This CSS File will be included in every
        /// page being delivered by the webpage
        /// </summary>
        /// <param name="manifestType">One type in which the assembly is hosted containing the CSS File</param>
        /// <param name="manifestName">Name of the Manifest</param>
        /// <param name="fileName">Filename under which the CSS File can be retrieved. The prefix 'css/' will
        /// be added by the logic</param>
        /// <param name="registrationType">Type of the registration</param>
        public void AddCssFileFromResource(
            Type manifestType,
            string manifestName,
            string fileName,
            RegistrationType registrationType = RegistrationType.EmbedAndLoad)
        {
            AddUrl(
                $"css/module/{fileName}",
                "text/css",
                () =>
                {
                    var result = manifestType.GetTypeInfo()
                        .Assembly.GetManifestResourceStream(manifestName);
                    return result ?? throw new InvalidOperationException($"The manifest {manifestName} was not found");
                });

            if (registrationType == RegistrationType.EmbedAndLoad)
            {
                _data.CssFiles.Add(fileName);
            }

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