using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Interface.Identifiers;

public interface IUriExtent : IExtent
{
    string contextURI();

    string? uri(IElement element);
    IElement? element(string uri);
}