using System.Threading.Tasks;

namespace PortaCapena.Authentication.Core
{
    public interface ITokenRefresher
    {
        /// <summary>
        /// Put given newToken into response header with given exchange token name set in <see cref="ITokenOptionsBuilder"/>
        /// </summary>
        /// <param name="newToken">New generated token</param>
        Task RefreshTokenAsync(string newToken);
    }
}
