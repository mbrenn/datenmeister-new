namespace DatenMeister.EMOF.Interface.Reflection
{
    /// <summary>
    ///     Interface for the Element according to  MOF CoreSpecification 2.5, Clause 9.2
    /// </summary>
    public interface IElement : IObject
    {
        /// <summary>
        /// Gets the metaclass of the element
        /// </summary>
        IElement metaclass { get; }

        /// <summary>
        /// Same as metaclass
        /// </summary>
        /// <returns>Element containing the element</returns>
        IElement getMetaClass();

        /// <summary>
        /// Gets the element containing the element
        /// </summary>
        /// <returns>Containing element</returns>
        IElement container();
    }
}