using System.Threading.Tasks;
using PortaCapena.Authentication.NetCore.Exceptions;

namespace PortaCapena.Authentication.NetCore.Abstraction
{
    public interface ITokenValidator
    {
        Task OnUnauthorizedAsync(AuthException exception);
    }
}
