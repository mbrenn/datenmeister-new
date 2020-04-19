using System.Collections.Generic;

namespace DatenMeister.Models.Forms
{
    public class DetailForm : Form
    {
        public DetailForm()
        {
        }

        public DetailForm(string name) : base(name)
        {
        }

        public DetailForm(string name, params FieldData[] fieldsToBeAdded) : this(name)
        {
            AddFields(fieldsToBeAdded);
        }

        /// <summary>
        /// Stores the default button text being by the user to acknowledge the action behind the form
        /// </summary>
        public string? buttonApplyText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the detailform allows new properties
        /// </summary>
        public bool allowNewProperties { get; set; }

        /// <summary>
        /// Gets or sets the default width in pixels for a standard 96dpi screen
        /// </summary>
        public int defaultWidth { get; set; }

        /// <summary>
        /// Gets or sets the default height in pixels for a standard 96 dpi screen
        /// </summary>
        public int defaultHeight { get; set; }

        /// <summary>
        /// Gets or sets the tabs
        /// </summary>
        public List<Form>? tab { get; set; }

        /// <summary>
        /// Stores the fields which shall be shown in the as the first field
        /// </summary>
        public IList<FieldData> field { get; set; } = new List<FieldData>();
        
        /// <summary>
        /// Adds the fields to the form
        /// </summary>
        /// <param name="fieldsToBeAdded">Fields to be added</param>
        public void AddFields(params FieldData[] fieldsToBeAdded)
        {
            foreach (var fieldToBeAdded in fieldsToBeAdded)
            {
                field.Add(fieldToBeAdded);
            }
        }
    }
}