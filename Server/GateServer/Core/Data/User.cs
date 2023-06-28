using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synchronize.Game.Lockstep.GateServer.Core.Data
{
    public class User
    {
        public string UserId;
        public NetPeer Peer;
        public User()
        {

        }
    }
}
