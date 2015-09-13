using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Interface.Identifiers
{
    public interface IUriExtent : IExtent
    {
        string contextURI();

        string uri(IElement element);

        IElement element(string uri);
    }
}
