using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PortaCapena.Authentication.NetCore
{
    public static class PcAuthInitializer
    {
        public static void Initialize(TokenOptions tokenOptions)
        {
            TokenManager.TokenOptions = tokenOptions;

            TokenManager.TokenValidationParamers = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.SecretKey)),
                ValidateIssuer = false,
                ValidIssuer = null,
                ValidateAudience = false,
                ValidAudience = null,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                
            };
        }
    }
}
