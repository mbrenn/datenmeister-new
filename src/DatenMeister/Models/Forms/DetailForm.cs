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
    }
}