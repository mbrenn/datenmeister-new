namespace DatenMeister.EMOF.Interface.Reflection
{
    public interface IElementExt
    {
        /// <summary>
        ///     Offers to set the meta class after creation
        /// </summary>
        /// <param name="metaClass">Metaclass to be set</param>
        void setMetaClass(IElement metaClass);
    }
}