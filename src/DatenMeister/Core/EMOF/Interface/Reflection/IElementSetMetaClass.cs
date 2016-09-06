namespace DatenMeister.Core.EMOF.Interface.Reflection
{
    public interface IElementSetMetaClass
    {
        /// <summary>
        ///     Offers to set the meta class after creation
        /// </summary>
        /// <param name="metaClass">Metaclass to be set</param>
        void setMetaClass(IElement metaClass);

        /// <summary>
        /// Sets the container of the given element in a hard way
        /// </summary>
        /// <param name="container">Container to be set</param>
        void setContainer(IElement container);
    }
}