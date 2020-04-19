using System.Collections.Generic;

namespace DatenMeister.Models.Forms
{
    public class ExtentForm : Form
    {
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