using System.Collections;
using System.Collections.Generic;

namespace DatenMeister.Web.Models.Fields
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
    }
}