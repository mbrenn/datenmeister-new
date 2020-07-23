namespace DatenMeister.Integration
{
    public interface IScopeStorage
    {
        /// <summary>
        /// Adds an item to the storage which is stored during the running instance.
        /// If an item of the specific type is already existing, it will be overwritten
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <typeparam name="T">Type of the item to be added</typeparam>
        void Add<T>(T item);

        /// <summary>
        /// Gets the storage item which was added via AddStorage Item.
        /// An exception will be thrown if the item cannot be found
        /// </summary>
        /// <typeparam name="T">Type to be retrieved</typeparam>
        /// <returns>The found storage item</returns>
        T Get<T>() where T : new();

        /// <summary>
        /// Tries to get a value and returns null, if value is not found
        /// </summary>
        /// <typeparam name="T">Type to be retrieved</typeparam>
        /// <returns>The found storage item or null, if not found</returns>
        T TryGet<T>();
    }
}