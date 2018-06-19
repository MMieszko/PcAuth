using PortaCapena.Authentication.NetCore;

namespace NetCore.AuthSample.Auth
{
    public class AdminRequirement : PcIdentityRequirement<AdminRole>
    {
        public AdminRequirement()
        {

        }
    }
}
