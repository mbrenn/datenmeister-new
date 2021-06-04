#nullable enable

using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Validators
{
    /// <summary>
    /// This interface is used to validate the state of an element.
    /// This is used by applications to give better response to the user
    /// </summary>
    public interface IElementValidator
    {
        /// <summary>
        /// Validates the properties and state of the given element
        /// </summary>
        /// <param name="element">Element to be verified</param>
        /// <returns>The result of the validation</returns>
        ValidatorResult? ValidateElement(IObject element);
    }
}