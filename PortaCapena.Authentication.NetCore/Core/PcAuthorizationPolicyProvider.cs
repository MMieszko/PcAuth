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

        public virtual async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return await new DefaultAuthorizationPolicyProvider(Options).GetPolicyAsync(policyName);
        }

        public virtual async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            await Task.Delay(1);

            return PcDefaultPolicy.Create;
        }
    }
}