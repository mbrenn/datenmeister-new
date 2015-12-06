using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.EMOF.Helper
{
    public static class UmlHelper
    {
        public static string GetName(IObject value)
        {
            if (value.isSet("name"))
            {
                return value.get("name").ToString();
            }

            return null;
        }
    }
}