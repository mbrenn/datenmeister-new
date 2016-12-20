namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Performs an allocation between the view and a specific metaclass which supports the retrieval of default views. 
    /// </summary>
    public class DefaultViewForExtent
    {
        public string extent { get; set; }

        public Form view { get; set; }

        public DefaultViewForExtent()
        {
        }

        public DefaultViewForExtent(string extent)
        {
            this.extent = extent;
        }

        public DefaultViewForExtent(string extent, Form form) : this(extent)
        {
            this.view = form;
        }

        public override string ToString()
        {
            return $"View for: {extent}";
        }
    }
}