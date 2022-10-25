using System;
using System.Collections.Generic;
using System.Text;

namespace RoomServer.Core.Data
{
    public class StartupConfig
    {
        public uint MapId;
        public int Port;
        public ushort PlayerNumber;
        public string GateServerName;
        public int GateWsPort;
        public string Hash;

        public void Update(uint mapId,int port,ushort playerNumber,string gateServerName,int gateWsPort,string hash)
        {
            MapId = mapId;
            Port = port;
            PlayerNumber = playerNumber;
            GateServerName = gateServerName;
            GateWsPort = gateWsPort;
            Hash = hash;
        }
    }
}
