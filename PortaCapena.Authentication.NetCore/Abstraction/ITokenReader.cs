using System.Security.Claims;
using System.Threading.Tasks;

namespace PortaCapena.Authentication.NetCore.Abstraction
{
    public interface ITokenReader
    {
        /// <summary>
        /// Sets given <see cref="ClaimsPrincipal" into <see cref="HttpContext.User"/>
        /// Also sets <see cref="Claims.UserId"/> claim into <see cref="HttpContext.Items"/> as UserId key/>
        /// </summary>
        /// <param name="principal">Current usser principals</param>
        Task FillHttpContextWithIdentity(ClaimsPrincipal principal);
    }
}
