using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PortaCapena.Authentication.NetCore.Configuration;

namespace PortaCapena.Authentication.NetCore.Core
{
    internal static class PcAuthInitializer
    {
        public static void Initialize(TokenOptions tokenOptions)
        {
            TokenManager.TokenOptions = tokenOptions;

            TokenManager.TokenValidationParameters = new TokenValidationParameters
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
