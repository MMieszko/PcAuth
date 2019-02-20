using PortaCapena.Authentication.NetCore.Core;

namespace NetCore.AuthSample.Auth
{
    public class AdminRequirement : PcIdentityRequirement<AdminRole>
    {
        public AdminRequirement()
        {

        }
    }
}
