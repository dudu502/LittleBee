using Logger;
using RoomServer.Services;
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
            ServerDll.Service.Modules.Service.GetModule<RoomServer.Modules.BattleModule>().InitStartup(mapId, playerNumber, 0, roomApplication.GetHashCode().ToString());
        }
        public static void Stop()
        {
            ServerDll.Service.Modules.Service.RemoveAllModule();
            if (roomApplication!=null)
            {
                roomApplication.ShutDown();
            }
            roomApplication = null;
            RoomApplication.Logger = null;
        }
    }
}
