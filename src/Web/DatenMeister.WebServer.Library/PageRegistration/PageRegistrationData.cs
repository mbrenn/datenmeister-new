using System.Collections.Generic;

namespace DatenMeister.WebServer.Library.PageRegistration
{
    /// <summary>
    /// Defines the data that is handled
    /// </summary>
    public class PageRegistrationData
    {
        /// <summary>
        /// Creates a list of page factories
        /// </summary>
        public List<PageFactory> PageFactories { get; } = new List<PageFactory>();
    }
}