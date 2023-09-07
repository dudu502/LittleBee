using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Gate
{
    public class GateAddressVO
    {
        public IPEndPoint RemotePoint;
        public string ConnectKey;
        public int HashCode;
    }
}
