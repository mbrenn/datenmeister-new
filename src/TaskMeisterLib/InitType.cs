namespace TaskMeister
{
    /// <summary>
    /// Defines the intialization type for the taskmeister
    /// </summary>
    public enum InitType
    {
        /// <summary>
        /// Just for local storage, non persistant
        /// </summary>
        NonPersistantGeneric = 0,

        /// <summary>
        /// Also for persistant storage into the xmi
        /// </summary>
        PersistantXmi = 1
    }
}