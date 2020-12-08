using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace PortaCapena.Authentication.NetCore.Core
{
    public class PcAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        protected readonly IOptions<AuthorizationOptions> Options;

        public PcAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            Options = options;
        }

        public virtual Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return new DefaultAuthorizationPolicyProvider(Options).GetPolicyAsync(policyName);
        }

        public virtual Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(PcDefaultPolicy.Create as AuthorizationPolicy);
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}