using PortaCapena.Authentication.NetCore.Abstraction;

namespace WebApi.Core
{
    public class AdminRole : Role
    {
        public override object Value => 1;

        public override string ToString() => Value.ToString();
    }
}
