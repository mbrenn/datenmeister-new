namespace DatenMeister.Models.Forms
{
    public class DefaultListViewForMetaclass
    {

        public ViewType viewType { get; set; }

        public string metaclass { get; set; }

        public Form view { get; set; }

        public DefaultListViewForMetaclass()
        {
        }

        public DefaultListViewForMetaclass(string metaClass, ViewType viewType)
        {
            this.metaclass = metaClass;
            this.viewType = viewType;
        }

        public DefaultListViewForMetaclass(string metaClass, ViewType viewType, Form form) : this(metaClass, viewType)
        {
            this.view = form;
        }

        public override string ToString()
        {
            return $"View for: {viewType} - {metaclass}";
        }
    }
}