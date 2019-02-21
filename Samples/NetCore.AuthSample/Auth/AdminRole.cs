using PortaCapena.Authentication.NetCore.Abstraction;

namespace NetCore.AuthSample.Auth
{
    public class AdminRole : Role
    {
        public override object Value => 1;

        public override string ToString() => Value.ToString();
    }
}
