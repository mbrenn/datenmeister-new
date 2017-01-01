namespace DatenMeister.Provider
{
    /// <summary>
    /// If the object reference is returned, an indirect reference to the object
    /// is returned. The id and metaclass Uri is contained within the class.
    /// The element is contained within the given database. 
    /// </summary>
    public class ElementReference
    {
        public string Id { get; set; }
        public string MetaclassUri { get; set; }
    }
}