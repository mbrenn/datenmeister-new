namespace DatenMeister.EMOF.Interface.Reflection
{
    /// <summary>
    /// This interface is used for all methods which are capable to set the container manually
    /// </summary>
    public interface IElementSetContainer
    {
        /// <summary>
        /// Sets the container of the given element
        /// </summary>
        /// <param name="element">Container element to be set for the element</param>
        void setContainer(IElement element);
    }
}