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
        public IEnumerable<string> GetUsers(int amount)
        {
            var result = new List<string>();

            for (var n = 0; n < amount; n++)
            {
                result.Add($"User {n}");
            }

            return result;

        }

        public IEnumerable<string> GetGroups(int amount)
        {
            var result = new List<string>();

            for (var n = 0; n < amount; n++)
            {
                result.Add($"Group {n}");
            }

            return result;

        }
    }
}
