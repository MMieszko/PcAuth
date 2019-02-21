using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PortaCapena.Authentication.NetCore.Core;

namespace PortaCapena.Authentication.NetCore.Abstraction
{
    public interface ITokenReader
    {
        /// <summary>
        /// Sets given <see cref="ClaimsPrincipal" into <see cref="HttpContext.User"/>
        /// Also sets <see cref="Claims.UserId"/> claim into <see cref="HttpContext.Items"/> as UserId key/>
        /// </summary>
        /// <param name="principal">Current user principals</param>
        /// <param name="context">Current http context</param>
        Task FillHttpContextWithIdentity(ClaimsPrincipal principal, HttpContext context);
    }
}
