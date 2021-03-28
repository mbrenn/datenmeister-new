using Microsoft.AspNetCore.Mvc;
using ZipCodeWebsite.Models;

namespace ZipCodeWebsite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZipCodeController : ControllerBase
    {
        public object Get(string? search)
        {
            return ZipCodeLogic.GetZipCodes(search);
        }
    }
}