using System;

namespace PortaCapena.Authentication.NetCore.Exceptions
{
    public class UnauthorizedException : AuthException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}