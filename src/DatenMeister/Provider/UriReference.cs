namespace DatenMeister.Provider
{
    /// <summary>
    /// If the object reference is returned, an indirect reference to the object
    /// is returned. The uri and metaclass Uri is contained within the class.
    /// The uri needs to be resolved by the MOF-Framework
    /// </summary>
    public class UriReference
    {
        public string Uri { get; set; }
    }
}