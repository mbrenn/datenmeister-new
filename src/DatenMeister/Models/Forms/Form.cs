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
        /// Gets or sets a value indicating whether the user may manipulate the view
        /// </summary>
        public bool fixView { get; set; }

        /// <summary>
        /// Gets or sets a value whether new values shall be allowed
        /// </summary>
        public bool inhibitNewItems { get; set; }

        /// <summary>
        /// Stores the name of the detail form which is used by default, when the user clicks 
        /// on one of the elements in the main form
        /// </summary>
        public string detailForm { get; set; }

        /// <summary>
        /// Indicate whether the meta class shall not be shown in the form.
        /// If the value is true, the detail form will not contain the metaclass
        /// </summary>
        public bool hideMetaClass { get; set; }

        /// <summary>
        /// Minimizes the navigation and reduces the content of the ribbons.
        /// </summary>
        public bool minimizeDesign { get; set; }

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