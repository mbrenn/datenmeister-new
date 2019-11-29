using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.UserInteractions;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.ZipExample
{
    public class ZipCodeInteractionHandler : BaseElementInteractionHandler
    {
        public override IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            if (IsRelevant(element))
            {
                var zipCode = element.GetOrDefault(nameof(ZipCode.zip))?.ToString();
                var name = element.GetOrDefault(nameof(ZipCode.name))?.ToString();

                if (string.IsNullOrEmpty(zipCode) || string.IsNullOrEmpty(name))
                    yield break;

                var query = $"{zipCode} {name}";
                yield return new DefaultElementInteraction(
                    "Open in OpenStreetMap",
                    () => Process.Start("https://www.openstreetmap.org/search?query="
                                        + WebUtility.UrlEncode(query)));
                yield return new DefaultElementInteraction(
                    "Open in Google Maps",
                    () => Process.Start("https://www.google.com/maps/search/?api=1&query="
                                        + WebUtility.UrlEncode(query)));
            }
        }
    }
}