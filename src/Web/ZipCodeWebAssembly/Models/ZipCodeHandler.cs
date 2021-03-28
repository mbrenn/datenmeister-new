using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ZipCodeWebsite.Models
{
    public class ZipCodeHandler
    {
        public static async Task<ZipCodeModel?> GetZipCodes(HttpClient httpClient, string? search)
        {
            var uri = "/zipcode/";
            if (!string.IsNullOrEmpty(search))
            {
                uri += "?search=" + search;
            }

            var result = await httpClient.GetFromJsonAsync<ZipCodeModel>(uri);

            return result;
        }
        
    }
}