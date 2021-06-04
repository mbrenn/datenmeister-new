namespace DatenMeister.Core.Provider
{
    /// <summary>
    /// Defines the possible object types for the DatenMeister.
    /// This object type is used as a hint for the object providers.
    /// </summary>
    public enum ObjectType
    {
        None,
        Boolean,
        Enum,
        String, 
        Integer,
        Double, 
        DateTime,
        Element,
        ReflectiveSequence
    }
}