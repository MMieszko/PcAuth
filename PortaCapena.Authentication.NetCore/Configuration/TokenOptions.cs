﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace PortaCapena.Authentication.NetCore
{
    public class TokenOptions
    {
        public string TokenName { get; internal set; }
        public string SecretKey { get; internal set; }
        public TimeSpan Expiration { get; internal set; }
        public SigningCredentials SigningCredentials { get; internal set; }
        public Func<Task<string>> NonceGenerator { get; internal set; }
        public string SecurityAlgorithm { get; internal set; }
        public IList<Claim> Claims { get; internal set; }
        public bool AutoRefresh { get; internal set; } = true;
        public string ExchangeTokenName { get; internal set; } = "X-Access-Token";

        public TokenOptions()
        {
            NonceGenerator = () => Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}