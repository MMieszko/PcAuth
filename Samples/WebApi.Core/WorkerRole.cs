using PortaCapena.Authentication.NetCore.Abstraction;

namespace WebApi.Core
{
    public class WorkerRole : Role
    {
        public override object Value => 100;
        public override string ToString() => Value.ToString();
    }
}
