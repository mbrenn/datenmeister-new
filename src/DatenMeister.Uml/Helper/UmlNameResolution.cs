using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Uml.Helper
{
    public class UmlNameResolution : IUmlNameResolution
    {
        public string GetName(IObject element)
        {
            if (element.isSet("name"))
            {
                return element.get("name").ToString();
            }
            else
            {
                return element.ToString();
            }
        }

        public string GetName(object element)
        {
            var asObject = element as IObject;
            return asObject == null ? element.ToString() : GetName(asObject);
        }
    }
}