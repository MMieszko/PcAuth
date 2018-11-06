using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Configuration;
using PortaCapena.Authentication.NetCore.Exceptions;

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
            catch (Exception ex)
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
            var claims = CreateDefaultClaims(userId, roles);

            return Create(claims);
        }

        /// <summary>
        /// Creates token for given userId and role
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="role">Role saved in token</param>
        /// <param name="keyValuePairs">Collection key pair values saved in tokenn</param>
        /// <returns>Jwt Token</returns>
        public static string Create(object userId, Role role, params KeyValuePair<string, string>[] keyValuePairs)
        {
            return Create(userId, new[] { role }, keyValuePairs);
        }

        /// <summary>
        /// Creates token for given userId and roles
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roles">Collection of roles saved in token</param>
        /// <param name="keyValuePairs">Collection key pair values saved in tokenn</param>
        /// <returns>Jwt Token</returns>
        public static string Create(object userId, Role[] roles, params KeyValuePair<string, string>[] keyValuePairs)
        {
            var defaultClaims = CreateDefaultClaims(userId, roles).ToList();
            var additionalClaims = CreateClaims(keyValuePairs).ToList();

            defaultClaims.AddRange(additionalClaims);

            return Create(defaultClaims);
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

        private static Claim[] CreateDefaultClaims(object userId, params Role[] roles)
        {
            var claims = new[]
            {
                new Claim(Claims.UserId, userId.ToString()),
                new Claim(Claims.Role, string.Join(",", roles.ToList()))
            };

            return claims;
        }

        private static Claim[] CreateClaims(IEnumerable<KeyValuePair<string, string>> keyPairValues)
        {
            return keyPairValues.Select(keyPair => new Claim(keyPair.Key, keyPair.Value)).ToArray();
        }
    }
}
