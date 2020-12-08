using PortaCapena.Authentication.NetCore.Core;

namespace WebApi.Core
{
    public class AdminRequirement : PcIdentityRequirement<AdminRole>
    {
        public AdminRequirement()
        {

        }
    }
}
