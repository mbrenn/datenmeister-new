using System.Collections.Generic;

namespace DatenMeister.Forms.FormCreator
{
    /// <summary>
    ///     A configuration helper class to create the extent form
    /// </summary>
    public class ExtentFormConfiguration
    {
        /// <summary>
        ///     Gets or sets the extent type to be used
        /// </summary>
        public List<string> ExtentTypes { get; set; } = new();
    }
}