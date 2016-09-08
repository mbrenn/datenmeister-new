using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public interface IUmlNameResolution
    {
        string GetName(IObject element);

        string GetName(object element);
    }
}