
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using Microsoft.AspNetCore.Mvc;

namespace ZipCodeWebsite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZipCodeController : ControllerBase
    {
        public object Get()
        {
            var elements = Program.ZipCodeExtent.elements();
            var data = new List<object>();

            foreach (var element in elements.OfType<IElement>())
            {
                data.Add(
                    new
                    {
                        id = element.getOrDefault<int>("id"),
                        name = element.getOrDefault<string>("name"),
                        zip = element.getOrDefault<int>("zip"),
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