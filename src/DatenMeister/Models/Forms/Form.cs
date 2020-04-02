using System.Collections.Generic;

namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Defines the view of
    /// </summary>
    public class Form
    {
        /// <summary>
        /// Gets or sets the name of the form
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// Gets or sets the title as shown in window header
        /// </summary>
        public string? title { get; set; }
        
        /// <summary>
        /// Gets or sets the information whether the complete form is readonly
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Indicate whether the meta information shall not be shown in the form.
        /// If the value is true, the detail form will not contain the metaclass
        /// </summary>
        public bool hideMetaInformation { get; set; }

        public Form()
        {
        }

        public Form(string name)
        {
            this.name = name;
        }
    }
}