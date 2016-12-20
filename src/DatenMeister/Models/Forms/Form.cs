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
        /// Stores the fields which shall be shown in the form
        /// </summary>
        public IList<FieldData> fields { get; set; } = new List<FieldData>();

        /// <summary>
        /// Stores the name of the detail form which is used by default, when the user clicks 
        /// on one of the elements in the main form
        /// </summary>
        public string detailForm { get; set; }

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
                fields.Add(field);
            }
        }
    }
}