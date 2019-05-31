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
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the title as shown in window header
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Stores the fields which shall be shown in the form
        /// </summary>
        public IList<FieldData> field { get; set; } = new List<FieldData>();

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

        public Form(string name, params FieldData[] fieldsToBeAdded) : this (name )
        {
            AddFields(fieldsToBeAdded);
        }

        /// <summary>
        /// Adds the fields to the form
        /// </summary>
        /// <param name="fieldsToBeAdded">Fields to be added</param>
        public void AddFields(params FieldData[] fieldsToBeAdded)
        {
            foreach (var field in fieldsToBeAdded)
            {
                this.field.Add(field);
            }
        }
    }
}