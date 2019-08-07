using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.AuthSample.Auth;
using PortaCapena.Authentication.NetCore.Core;
using PortaCapena.Authentication.NetCore.Extensions;

namespace NetCore.AuthSample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var uid = HttpContext.Items["UserId"];
            return new string[] { "value1", "value2" };
        }

        [HttpGet, Route("adminreq")]
        [Authorize(Policy = "AdminPolicy")]
        public string TestAdminRequirement()
        {
            var isInRole1 = User.IsInRole(new AdminRole());
            var isInRole2 = User.IsInRole(1);
            var userId1 = User.GetUserId();
            var userId2 = HttpContext.Items[Constants.UserId];

            return
                $"Is in Role1: {isInRole1} {Environment.NewLine} Is in Role2: {isInRole2} {Environment.NewLine} UserId1: {userId1} {Environment.NewLine} UserId2: {userId2}";
        }

        [HttpGet, Route("userreq")]
        [Authorize(Policy = "UserPolicy")]
        public string TestUserRequirement()
        {
            var isInRole1 = User.IsInRole(new AdminRole());
            var isInRole2 = User.IsInRole(1);

            return $"Is in Role1: {isInRole1} {Environment.NewLine} Is in Role2: {isInRole2}";
        }

        [HttpGet, Route("defaultpolicy")]
        [Authorize]
        public string TestDefaultPolicy()
        {
            return "Succeed";
        }

        [HttpGet, Route("multiplereq")]
        [Authorize(Policy = "AdminOrUserPolicy")]
        public string TestMultipleRolePolicy()
        {
            return "Succeed";
        }

        [AllowAnonymous]
        [HttpGet, Route("create/admin")]
        public string GenerateAdminToken()
        {
            var token = TokenManager.Create(1, new AdminRole(), new KeyValuePair<string, string>("A", "B"));

            var claimsPrincipal = TokenManager.Read(token);

            var aValue = claimsPrincipal.Claims.GetAssignedValue("A");

            return token;
        }

        [AllowAnonymous]
        [HttpGet, Route("create/user")]
        public string GenerateUserToken()
        {
            var token = TokenManager.Create(2, new UserRole(), new KeyValuePair<string, string>("C", "D"));

            var claimsPrincipal = TokenManager.Read(token);

            var cValue = claimsPrincipal.Claims.GetAssignedValue("B");

            return token;
        }

        [AllowAnonymous]
        [HttpGet, Route("create/worker")]
        public string GenerateWorkerToken()
        {
            var token = TokenManager.Create(3, new WorkerRole());

            var claimsPrincipal = TokenManager.Read(token);
            
            return token;
        }
    }
}
