using System.Collections.Generic;

namespace DatenMeister.Provider
{
    /// <summary>
    /// An object representing an object or element in MOF space
    /// </summary>
    public interface IProviderObject
    {
        bool IsPropertySet(string property);

        object GetProperty(string property);

        IEnumerable<string> GetProperties();

        bool DeleteProperty(string property);

        void SetProperty(string property, object value);

        bool AddToProperty(string property, object value);

        bool RemoveFromProperty(string property, object value);

    }
}