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

        public DetailForm(string name, params FieldData[] fieldsToBeAdded) : base(name, fieldsToBeAdded)
        {
        }

        /// <summary>
        /// Stores the default button text being by the user to acknowledge the action behind the form
        /// </summary>
        public string defaultApplyText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the detailform allows new properties
        /// </summary>
        public bool allowNewProperties { get; set; }
    }
}