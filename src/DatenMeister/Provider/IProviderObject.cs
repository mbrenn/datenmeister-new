using System.Collections.Generic;

namespace DatenMeister.Provider
{
    /// <summary>
    /// An object representing an object or element in MOF space
    /// </summary>
    public interface IProviderObject
    {
        /// <summary>
        /// Gets the id of the provided object 
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the uri of the metaclass or null, if not defined
        /// </summary>
        string MetaclassUri { get; }

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