
using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ZipCodeWebsite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZipCodeController : ControllerBase
    {
        public object Get(string? search)
        {
            if (Program.ZipCodeExtent == null)
            {
                return StatusCode(503);
            }
            
            IReflectiveCollection elements = Program.ZipCodeExtent.elements();
            var data = new List<object>();

            if (!string.IsNullOrEmpty(search))
            {
                elements = elements.WhenOneOfThePropertyContains(
                    new[] {"zip", "name"},
                    search,
                    StringComparison.CurrentCultureIgnoreCase);
            }

            foreach (var element in elements.OfType<IElement>()
                )
            {
                data.Add(
                    new
                    {
                        id = element.getOrDefault<int>("id"),
                        name = element.getOrDefault<string>("name"),
                        zip = element.getOrDefault<string>("zip"),
                        positionLong = element.getOrDefault<double>("positionLong"),
                        positionLat = element.getOrDefault<double>("positionLat")
                    });
            }

            return new
            {
                items = data
            };
        }
    }
}