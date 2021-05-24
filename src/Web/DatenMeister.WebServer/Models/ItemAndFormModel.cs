using System.Collections.Generic;

namespace DatenMeister.WebServer.Models
{
    public record ItemAndFormModel
    {
        public string form = string.Empty;

        public string item = string.Empty;

        /// <summary>
        /// Defines the name of the workspace of the model
        /// </summary>
        public string workspace = string.Empty;

        /// <summary>
        /// Defines the extent uri of the model
        /// </summary>
        public string extentUri = string.Empty;
        
        /// <summary>
        /// Gets or sets the fullname
        /// </summary>
        public string fullName = string.Empty;
    }
}