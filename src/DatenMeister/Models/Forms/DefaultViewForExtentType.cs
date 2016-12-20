namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Performs an allocation between the view and a specific metaclass which supports the retrieval of default views. 
    /// </summary>
    public class DefaultViewForExtentType
    {
        public string extentType { get; set; }

        public Form view { get; set; }

        public DefaultViewForExtentType()
        {
        }

        public DefaultViewForExtentType(string extentType)
        {
            this.extentType = extentType;
        }

        public DefaultViewForExtentType(string extentType, Form form) : this(extentType)
        {
            view = form;
        }

        public override string ToString()
        {
            return $"View for: {extentType}";
        }
    }
}