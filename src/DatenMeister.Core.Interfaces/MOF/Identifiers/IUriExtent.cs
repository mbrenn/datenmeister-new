using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Interfaces.MOF.Identifiers;

public interface IUriExtent : IExtent
{
    string contextURI();

    string? uri(IElement element);
    IElement? element(string uri);
}