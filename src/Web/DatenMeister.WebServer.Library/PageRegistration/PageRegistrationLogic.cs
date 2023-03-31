using System;
using System.IO;
using System.Linq;

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
        /// <param name="pageStreamFactory">Page Stream Factory to be used</param>
        public void AddUrl(string url, string contentType, Func<Stream> pageStreamFactory)
        {
            _data.PageFactories.Add(
                new PageFactory(url, contentType, pageStreamFactory));
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