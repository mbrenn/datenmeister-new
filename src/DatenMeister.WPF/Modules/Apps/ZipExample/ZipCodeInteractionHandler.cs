using System.Collections.Generic;
using System.Net;
using System.Web;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.Apps.ZipExample
{
    public class ZipCodeInteractionHandler : BaseElementInteractionHandler
    {
        public override IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            if (IsRelevant(element))
            {
                var zipCode = element.getOrDefault<string>(nameof(ZipCode.zip));
                var name = element.getOrDefault<string>(nameof(ZipCode.name));

                if (string.IsNullOrEmpty(zipCode) || string.IsNullOrEmpty(name))
                    yield break;

                var query = $"{zipCode} {name}";
                yield return new DefaultElementInteraction(
                    "Open in OpenStreetMap",
                    () => DotNetHelper.CreateProcess("https://www.openstreetmap.org/search?query="
                                        + HttpUtility.UrlEncode(query)));
                yield return new DefaultElementInteraction(
                    "Open in Google Maps",
                    () => DotNetHelper.CreateProcess("https://www.google.com/maps/search/?api=1&query="
                                                     + HttpUtility.UrlEncode(query)));
            }
        }
    }
}