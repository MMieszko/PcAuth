using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.AuthSample.Auth;
using PortaCapena.Authentication.NetCore;

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

        [AllowAnonymous]
        [HttpGet, Route("create")]
        public string GenerateToken()
        {
            var token = TokenManager.Create(1, new AdminRole(), new KeyValuePair<string, string>("Dzwig", "Tak"));

            var claimsPrincipal = TokenManager.Read(token);

            return token;
        }
    }
}
