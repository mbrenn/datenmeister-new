namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Defines an interface allowing access to a metaobject representing the current object
    /// </summary>
    public interface IHasMofExtentMetaObject
    {
        /// <summary>
        /// Gets the meta object representing the meta object. Setting, querying a list or getting
        /// is supported by this object
        /// </summary>
        /// <returns>The returned value representing the meta object</returns>
        MofObject GetMetaObject();
    }
}