using PortaCapena.Authentication.NetCore.Abstraction;

namespace WebApi.Core
{
    public class UserRole : Role
    {
        public override object Value => 10;

        public override string ToString() => Value.ToString();
    }
}