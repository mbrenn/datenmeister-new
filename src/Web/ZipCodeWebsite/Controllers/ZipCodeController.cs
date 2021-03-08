
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ZipCodeWebsite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZipCodeController : ControllerBase
    {
        public IEnumerable<string> Get()
        {
            return new List<string>
            {
                "Mainz",
                "Frankfurt",
                "Berlin"
            };
        }
    }
}