using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using PortaCapena.Authentication.NetCore.Abstraction;
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
        /// Retrieve roles values as string array 
        /// </summary>
        /// <param name="this">Collection of <see cref="Claim"/></param>
        /// <returns>Role values as string inside array</returns>
        public static string[] GetRolesArray(this IEnumerable<Claim> @this)
        {
            var values = @this.GetRoleValues();

            if (string.IsNullOrEmpty(values))
                return null;

            var array = values.Split(',');

            return array;
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

        /// <summary>
        ///Retrieve claim value added while creating token using <see cref="TokenManager"/> Create() method in KeyPairValue parameter
        /// </summary>
        /// <param name="this">Collection of <see cref="Claim"/></param>
        /// <param name="key">Name of key</param>
        /// <returns>Value of given key</returns>
        public static string GetAssignedValue(this IEnumerable<Claim> @this, string key)
        {
            return @this.SingleOrDefault(x => x.Type == key)?.Value;
        }
    }
}
