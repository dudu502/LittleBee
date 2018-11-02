using System;
using System.Collections.Generic;

namespace NetServiceImpl.Server.Data
{
    public class GamePlayerSvo
    {
        public long Id;
        public GamePlayerSvo()
        {
            Id = GetHashCode();
        }
    } 
}
