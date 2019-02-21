using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PortaCapena.Authentication.NetCore.Abstraction
{
    public interface ITokenRefresher
    {
        /// <summary>
        /// Put given newToken into response header with given exchange token name set in <see cref="ITokenOptionsBuilder"/>
        /// </summary>
        /// <param name="newToken">New generated token</param>
        /// <param name="httpContext">Current HttpContext</param>
        Task RefreshTokenAsync(string newToken, HttpContext httpContext);
    }
}
