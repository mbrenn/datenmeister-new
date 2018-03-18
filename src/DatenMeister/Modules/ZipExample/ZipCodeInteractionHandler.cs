using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.UserInteractions;

namespace DatenMeister.Modules.ZipExample
{
    public class ZipCodeInteractionHandler :  BaseElementInteractionHandler
    {
        public ZipCodeInteractionHandler()
        {
                
        }

        public override IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            if (IsRelevant(element))
            {
                var zipCode = element.getOrDefault(nameof(ZipExampleController.ZipCodeModel.Zip))?.ToString();
                var name = element.getOrDefault(nameof(ZipExampleController.ZipCodeModel.CityName))?.ToString();

                if (string.IsNullOrEmpty(zipCode) || string.IsNullOrEmpty(name))
                {
                    yield break;
                }

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