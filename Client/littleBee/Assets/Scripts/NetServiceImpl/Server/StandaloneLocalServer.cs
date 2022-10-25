using Logger;
using RoomServer.Core.Data;
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
        public static void Start(string key, int port, uint mapId, ushort playerNumber)
        {
            Stop();
            var logger = new UnityEnvLogger(key);
            RoomApplication.Logger = logger;
            roomApplication = new RoomApplication(port);
            roomApplication.StartServer();
            DataMgr.Instance.BattleSession.StartupCFG.Update(mapId, port, playerNumber, string.Empty, 0, Guid.NewGuid().ToString());
        }
        public static void Stop()
        {
           if (roomApplication!=null)
           {
               roomApplication.ShutDown();
           }
           roomApplication = null;
           RoomApplication.Logger = null;
        }
    }
}
