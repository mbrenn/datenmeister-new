using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Interfaces;

public interface IElementWrapper
{
    IElement GetWrappedElement();
}