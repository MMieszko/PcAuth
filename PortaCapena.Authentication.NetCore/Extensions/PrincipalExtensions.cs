using System.Linq;
using System.Security.Claims;
using PortaCapena.Authentication.NetCore.Abstraction;

namespace PortaCapena.Authentication.NetCore.Extensions
{
    public static class PrincipalExtensions
    {
        /// <summary>
        /// Check if this user contain the role.
        /// </summary>
        /// <param name="this">Claims</param>
        /// <param name="roles">Roles to check</param>
        public static bool IsInRole(this ClaimsPrincipal @this, params Role[] roles)
        {
            return @this.IsInRole(roles.Select(role => role.Value).ToArray());
        }

        /// <summary>
        /// Check if this user contain the role.
        /// </summary>
        /// <param name="values">Values of <see cref="Role"/></param>
        /// <param name="this">Claims</param>
        public static bool IsInRole(this ClaimsPrincipal @this, params object[] values)
        {
            var rolesArray = @this.Claims.GetRolesArray();

            if (rolesArray == null)
                return false;

            foreach (var role in values)
            {
                if (!rolesArray.Contains(role.ToString()))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieve user id.
        /// </summary>
        /// <returns></returns>
        public static string GetUserId(this ClaimsPrincipal @this)
        {
            return @this.Claims.GetUserIdValue();
        }
    }
}