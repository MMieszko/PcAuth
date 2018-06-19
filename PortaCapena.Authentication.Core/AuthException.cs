using System;

namespace PortaCapena.Authentication.Core
{
    public class AuthException : Exception
    {
        public AuthException(string message)
            :base(message)
        {
            
        }

        public AuthException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
