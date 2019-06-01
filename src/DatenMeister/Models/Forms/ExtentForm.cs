using System.Collections;
using System.Collections.Generic;

namespace DatenMeister.Models.Forms
{
    public class ExtentForm
    {
        /// <summary>
        /// Gets or sets the name of the form
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the title as shown in window header
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Stores a list of tabs forms that will be added
        /// </summary>
        public List<Form> tab { get; set; } = new List<Form>();

        /// <summary>
        /// True, if tabs shall be created automatically for each metaclass in the extent. 
        /// </summary>
        public bool autoTabs { get; set; }
    }
}