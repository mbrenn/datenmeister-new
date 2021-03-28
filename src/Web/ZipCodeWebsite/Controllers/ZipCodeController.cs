
using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
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