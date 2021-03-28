using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;

namespace ZipCodeWebsite.Models
{
    public class ZipCodeLogic
    {
        public static ZipCodeModel GetZipCodes(string? search)
        {
            if (Program.ZipCodeExtent == null)
            {
                throw new InvalidOperationException("ZipCodeExtent not booted up");
            }
            
            IReflectiveCollection elements = Program.ZipCodeExtent.elements();
            var data = new List<ZipCodeData>();

            if (!string.IsNullOrEmpty(search))
            {
                elements = elements.WhenOneOfThePropertyContains(
                    new[] {"zip", "name"},
                    search,
                    StringComparison.CurrentCultureIgnoreCase);
            }

            foreach (var element in elements.OfType<IElement>().Take(101))
            {
                data.Add(
                    new ZipCodeData
                    {
                        id = element.getOrDefault<int>("id"),
                        name = element.getOrDefault<string>("name"),
                        zip = element.getOrDefault<string>("zip"),
                        positionLong = element.getOrDefault<double>("positionLong"),
                        positionLat = element.getOrDefault<double>("positionLat")
                    });
            }

            return new ZipCodeModel
            {
                items = data.Take(100).ToList(),
                truncated = data.Count >= 100,
                noItemFound = data.Count == 0
            };
        }
        
    }
}