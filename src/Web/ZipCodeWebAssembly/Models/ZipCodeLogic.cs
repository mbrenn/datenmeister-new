using System.Net.Http.Json;
using ZipCodeLibrary;

namespace ZipCodeWebAssembly.Models
{
    public static class ZipCodeLogic
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