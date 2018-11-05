using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PortaCapena.Authentication.NetCore
{
    public static class TokenManager
    {
        internal static TokenValidationParameters TokenValidationParamers { get; set; }

        /// <summary>
        /// TokenOptions created using <see cref="ITokenOptionsBuilder"/>
        /// </summary>
        public static TokenOptions TokenOptions { get; internal set; }

        /// <summary>
        /// TokenOptions created using <see cref="ITokenOptionsBuilder"/>
        /// </summary>
        public static ClaimsPrincipal Read(string token)
        {
            ClaimsPrincipal principal;
            var handler = new JwtSecurityTokenHandler();

            try
            {
                principal = handler.ValidateToken(token, TokenValidationParamers, out var validToken);
                if (!(validToken is JwtSecurityToken))
                    throw new AuthException("Given token is not valid");
            }
            catch (SecurityTokenValidationException ex)
            {
                throw new AuthException(ex.Message, ex);
            }
            catch(Exception ex)
            {
                throw new AuthException("Unexpected exception while trying to read token. See inner exception for more details", ex);
            }

            return principal;
        }
       
        /// <summary>
        /// Creates token for given userId and roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roles">Collection of roles saved in token</param>
        /// <returns>Jwt Token</returns>
        public static string Create(object userId, params Role[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(Claims.UserId, userId.ToString()),
                new Claim(Claims.Role, string.Join(",", roles.ToList()))
            };

            return Create(claims);
        }

        /// <summary>
        /// Creates token for given userId and roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roles">Collection of roles saved in token</param>
        /// <returns>Jwt token</returns>
        public static string Create(object userId, params string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(Claims.UserId, userId.ToString()),
                new Claim(Claims.Role, string.Join(",", roles.ToList()))
            };

            return Create(claims);
        }

        /// <summary>
        /// Creates token based on existing claims
        /// </summary>
        /// <param name="claims">Colection of claims</param>
        /// <returns>Jwt token</returns>
        public static string Create(IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(null, null, claims, DateTime.UtcNow, DateTime.UtcNow.Add(TokenOptions.Expiration), TokenOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
