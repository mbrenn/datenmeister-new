using System.Collections.Generic;

namespace DatenMeister.Web.Models.Forms
{
    /// <summary>
    /// Defines the view of 
    /// </summary>
    public class Form
    {
        /// <summary>
        /// Gets or sets the name of the form
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Stores the fields which shall be shown in the form
        /// </summary>
        public IList<FieldData> fields { get; set; } = new List<FieldData>();

        /// <summary>
        /// Stores the name of the detail form which is used by default, when the user clicks 
        /// on one of the elements in the main form
        /// </summary>
        public string detailForm { get; set; }
    }
}