using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PortaCapena.Authentication.NetCore.Abstraction;
using PortaCapena.Authentication.NetCore.Configuration;
using PortaCapena.Authentication.NetCore.Core;

namespace PortaCapena.Authentication.NetCore.Extensions
{
    public static class ClaimExtensions
    {
        /// <summary>
        /// Retrieve <see cref="Claims.Role"/> values from given collection of claims
        /// </summary>
        /// <param name="this">Collection of <see cref="Claim"/></param>
        /// <returns>String value from <see cref="Role"/></returns>
        public static string GetRoleValues(this IEnumerable<Claim> @this)
        {
            return @this?.SingleOrDefault(x => x.Type.Contains(Claims.Role))?.Value;
        }

        /// <summary>
        /// Retrieve <see cref="Claims.UserId"/> value from given collection of claims
        /// </summary>
        /// <param name="this">Collection of <see cref="Claim"/></param>
        /// <returns>String value from <see cref="Role"/></returns>
        public static string GetUserIdValue(this IEnumerable<Claim> @this)
        {
            return @this?.SingleOrDefault(x => x.Type.Contains(Claims.UserId))?.Value;
        }
    }
}
