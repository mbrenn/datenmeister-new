namespace DatenMeister.Provider.ManualMapping
{
    /// <summary>
    /// Stores just a small helper interface to retrieve the value in an template value independent way
    /// </summary>
    internal interface IHasValue
    {
        object ValueAsObject { get; }
    }
}