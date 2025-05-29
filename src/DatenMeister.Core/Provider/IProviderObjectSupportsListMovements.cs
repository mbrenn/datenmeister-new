namespace DatenMeister.Core.Provider;

/// <summary>
/// This interface shall be implemented by all provider objects which directly support the movement
/// of objects within the lists. It speeds up the movement and avoids the complete removing and inserting
/// of elements when user or an application requires the movement
/// </summary>
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