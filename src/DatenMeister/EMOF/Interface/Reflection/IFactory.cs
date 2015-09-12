namespace DatenMeister.EMOF.Interface.Reflection
{
    /// <summary>
    /// Interface to Factory according to MOFSpecification 2.5, Clause 9.3
    /// </summary>
    public interface IFactory
    {
        IElement package
        {
            get;
        }

        IElement create(IElement metaClass);

        IObject createFromString(IElement dataType, string value);

        string convertToString(IElement dataType, IObject value);
    }
}
