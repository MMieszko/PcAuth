using NetWebApi.AuthSample.Auth;
using PortaCapena.Authentication.Net;
using System.Web;
using System.Web.Http;

namespace NetWebApi.AuthSample.Controllers
{
    [AuthenticationActionFilter(Roles.AdminRole, Roles.SuperAdminRole)]
    public class SimpleAuthController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var userId = HttpContext.Current.Items["UserId"];
            return Ok("Czesc");
        }
    }
}