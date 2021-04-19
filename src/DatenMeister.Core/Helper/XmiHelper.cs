using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;

namespace DatenMeister.Core.Helper
{
    public class XmiHelper
    {
        public static string ConvertToXmi(IObject value)
        {
            var provider = new XmiProvider();
            var extent = new MofUriExtent(provider);

            var copiedResult = ObjectCopier.Copy(new MofFactory(extent), value);
            var providerObject = (copiedResult as MofObject)?.ProviderObject;
            var xml = (providerObject as XmiProviderObject)?.XmlNode;

            if (xml == null)
            {
                throw new InvalidOperationException("xml not found");
            }
            
            return xml.ToString();
        }
    }
}