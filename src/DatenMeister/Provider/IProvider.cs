using System;
using System.Collections.Generic;

namespace DatenMeister.Provider
{
    /// <summary>
    /// Defines the flags self-describing the provider and its capabilities
    /// </summary>
    [Flags]
    public enum ProviderCapability
    {
        /// <summary>
        /// No special capability
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Will be set, if the provider is capable to store the information about possible meta information within the
        /// extent itself. Is this flag is not set, the data will be stored by the extent itself and saved within the
        /// uri extent loading file
        /// </summary>
        StoreMetaDataInExtent = 0x01
    }

    /// <summary>
    /// Defines the interface as required for the provider.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Creates a temporary element, which is not permanently added to the database.
        /// If the element shall be added, the method <c>AddElement</c> needs to be used
        /// </summary>
        /// <param name="metaClassUri">Gets the metaclass uri of the element</param>
        /// <returns>The created object</returns>
        IProviderObject CreateElement(string metaClassUri);

        /// <summary>
        /// Adds an element to the provider
        /// </summary>
        /// <param name="valueAsObject">Element to be provided</param>
        /// <param name="index">Index position where the element shall be stored, if -1, the element will be stored at end of the extent</param>
        void AddElement(IProviderObject valueAsObject, int index = -1);

        /// <summary>
        /// Deletes a single element with the given id from database
        /// </summary>
        /// <param name="id">Id of the object to be deleted</param>
        /// <returns>True, if the element was deleted</returns>
        bool DeleteElement(string id);

        /// <summary>
        /// Deletes all elements from the database
        /// </summary>
        void DeleteAllElements();

        /// <summary>
        /// Gets the object with the given id
        /// </summary>
        /// <param name="id">Id of the object being queried</param>
        /// <returns>The found object or null, if not found</returns>
        IProviderObject Get(string id);

        /// <summary>
        /// Gets all objects that are at root at the element
        /// </summary>
        /// <returns>All objects at root</returns>
        IEnumerable<IProviderObject> GetRootObjects();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProviderCapability GetCapabilities();
    }
}