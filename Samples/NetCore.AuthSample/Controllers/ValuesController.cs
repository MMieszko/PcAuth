using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.AuthSample.Auth;

namespace NetCore.AuthSample.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = nameof(AdminRequirement))]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var uid = HttpContext.Items["UserId"];
            return new string[] { "value1", "value2" };
        }
    }
}
