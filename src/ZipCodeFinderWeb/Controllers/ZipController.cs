using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ZipCodeFinderWeb.Controllers
{
    public class ZipController : ApiController
    {
        public IEnumerable<string> Get(string zip)
        {
            var result = new List<string>();
            result.Add($"ABS {zip}");
            return result;
        }
    }
}
