using System.Threading.Tasks;

namespace PortaCapena.Authentication.NetCore.Abstraction
{
    public interface ITokenValidator
    {
        /// <summary>
        /// Throws <see cref="AuthException"/> with given message
        /// </summary>
        /// <param name="message">Exception message</param>
        Task OnUnauthorizedAsync(string message);
    }
}
