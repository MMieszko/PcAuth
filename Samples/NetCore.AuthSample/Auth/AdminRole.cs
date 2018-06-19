using PortaCapena.Authentication.Core;

namespace NetCore.AuthSample.Auth
{
    public class AdminRole : Role
    {
        public override object Value => 2;

        public override string ToString() => Value.ToString();
    }
}
