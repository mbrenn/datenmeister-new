namespace DatenMeister.Models.Forms
{
    /// <summary>
    /// Performs an allocation between the view and a specific metaclass which supports the retrieval of default views. 
    /// </summary>
    public class DefaultDetailViewForMetaclass
    {
        public ViewType viewType { get; set; } 

        public string metaclass { get; set; }
        
        public Form view { get; set; }

        public DefaultDetailViewForMetaclass()
        {
        }

        public DefaultDetailViewForMetaclass(string metaClass, ViewType viewType)
        {
            this.metaclass = metaClass;
            this.viewType = viewType;
        }

        public DefaultDetailViewForMetaclass(string metaClass, ViewType viewType, Form form) : this(metaClass, viewType)
        {
            this.view = form;
        }

        public override string ToString()
        {
            return $"View for: {viewType} - {metaclass}";
        }
    }
}