
using Synchronize.Game.Lockstep.Logger;
using Synchronize.Game.Lockstep.RoomServer.Modules;
using Synchronize.Game.Lockstep.RoomServer.Services;
using Synchronize.Game.Lockstep.Service.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 单机本地服务器 启动器
/// 逻辑部分Copy自RoomServer
/// </summary>
namespace NetServiceImpl.Server
{
    public class StandaloneLocalServer
    {
        private static RoomApplication roomApplication;
        public static void Start(string key,int port,uint mapId,ushort playerNumber)
        {
            Stop();
            var logger = new UnityEnvLogger("Standalone");
            RoomApplication.Logger = logger;
            roomApplication = new RoomApplication(key);
            roomApplication.StartServer(port);
            BaseModule.GetModule<BattleModule>().InitStartup(mapId, playerNumber, 0, roomApplication.GetHashCode().ToString());
        }
        public static void Stop()
        {
            BaseModule.RemoveAllModules();
            if (roomApplication!=null)
            {
                roomApplication.ShutDown();
                roomApplication.Dispose();
            }
            roomApplication = null;
            RoomApplication.Logger = null;
        }
    }
}
