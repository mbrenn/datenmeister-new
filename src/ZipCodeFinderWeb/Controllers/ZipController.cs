using DatenMeister.App.ZipCode;
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
        public IEnumerable<object> Get(string zip)
        {
            if (string.IsNullOrEmpty(zip))
            {
                zip = string.Empty;
            }

            var result = new List<object>();
            var found = DataProvider.TheOne.FindBySearchString(zip).Take(100);
            foreach (var foundObject in found)
            {
                result.Add(
                    new
                    {
                        Zipcode = foundObject.get(DataProvider.Columns.ZipCode),
                        City = foundObject.get(DataProvider.Columns.Name)
                    });
            }
            
            return result;
        }
    }
}
