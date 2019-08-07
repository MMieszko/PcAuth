using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortaCapena.Authentication.NetCore.Abstraction;

namespace NetCore.AuthSample.Auth
{
    public class WorkerRole : Role
    {
        public override object Value => 100;
        public override string ToString() => Value.ToString();
    }
}
