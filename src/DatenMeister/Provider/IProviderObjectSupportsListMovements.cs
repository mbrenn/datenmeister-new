namespace DatenMeister.Provider
{
    public interface IProviderObjectSupportsListMovements
    {
        /// <summary>
        /// Moves the given element up by one position in the property's list 
        /// </summary>
        /// <param name="property">Property in whose list the value is given </param>
        /// <param name="value">Value to be moved up</param>
        /// <returns>true, if movement was successful</returns>
        bool MoveElementUp(string property, object value);
        
        /// <summary>
        /// Moves the given element down by one position in the property's list 
        /// </summary>
        /// <param name="property">Property in whose list the value is given </param>
        /// <param name="value">Value to be moved down</param>
        /// <returns>true, if movement was successful</returns>
        bool MoveElementDown(string property, object value);
    }
}