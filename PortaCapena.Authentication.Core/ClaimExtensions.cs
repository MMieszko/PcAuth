﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PortaCapena.Authentication.Core
{
    public static class ClaimExtensions
    {
        /// <summary>
        /// Retrive <see cref="Claims.Role"/> values from given collection of claims
        /// </summary>
        /// <param name="this">Collection of <see cref="Claim"/></param>
        /// <returns>String value from <see cref="Role"/></returns>
        public static string GetRoleValues(this IEnumerable<Claim> @this)
        {
            return @this?.SingleOrDefault(x => x.Type.Contains(Claims.Role))?.Value;
        }

        /// <summary>
        /// Retrive <see cref="Claims.UserId"/> value from given collection of claims
        /// </summary>
        /// <param name="this">Collection of <see cref="Claim"/></param>
        /// <returns>String value from <see cref="Role"/></returns>
        public static string GetUserIdValue(this IEnumerable<Claim> @this)
        {
            return @this?.SingleOrDefault(x => x.Type.Contains(Claims.UserId))?.Value;
        }
    }
}