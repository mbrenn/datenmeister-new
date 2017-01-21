using System.Collections.Generic;

namespace DatenMeister.Provider
{
    /// <summary>
    /// An object representing an object or element in MOF space
    /// </summary>
    public interface IProviderObject
    {
        /// <summary>
        /// Gets the corresponding provider of the object, which has created and might storing the object
        /// </summary>
        IProvider Provider { get; }

        /// <summary>
        /// Gets or sets the id of the provided object 
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets the uri of the metaclass or null, if not defined
        /// </summary>
        string MetaclassUri { get; set; }

        bool IsPropertySet(string property);

        object GetProperty(string property);

        IEnumerable<string> GetProperties();

        bool DeleteProperty(string property);

        void SetProperty(string property, object value);

        /// <summary>
        /// Empties a list of a property and prepares the property that there is a call for <c>AddProperty</c>. 
        /// </summary>
        /// <param name="property">Property to be emptied</param>
        void EmptyListForProperty(string property);

        bool AddToProperty(string property, object value, int index = -1);

        bool RemoveFromProperty(string property, object value);

    }
}