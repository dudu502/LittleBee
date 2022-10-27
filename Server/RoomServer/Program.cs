using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Linq;
using Service.Core;
using RoomServer.Services;
using System.Threading;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Service.HttpMisc;
using RoomServer.Core.Data;
using ServerDll.Service;

namespace RoomServer
{
    class Program
    {

        static void Main(string[] args)
        {
            string key = "SomeConnectionKey";
            uint mapId = 1;
            ushort port = 60000;
            ushort playerNumber = 100;
            string gateWsServerName = string.Empty;
            ushort gateWsPort = 0;
            string hash = "";
            if (args.Length > 0)
            {
                if (Array.IndexOf(args, "-port") > -1) port = Convert.ToUInt16(args[Array.IndexOf(args, "-port") + 1]);
                if (Array.IndexOf(args, "-mapId") > -1) mapId = Convert.ToUInt32(args[Array.IndexOf(args, "-mapId") + 1]);
                if (Array.IndexOf(args, "-playernumber") > -1) playerNumber = Convert.ToUInt16(args[Array.IndexOf(args, "-playernumber") + 1]);
                if (Array.IndexOf(args, "-gateWsServerName") > -1) gateWsServerName = args[Array.IndexOf(args, "-gateWsServerName") + 1];
                if (Array.IndexOf(args, "-gateWsPort") > -1) gateWsPort = Convert.ToUInt16( args[Array.IndexOf(args, "-gateWsPort") + 1]);
                if (Array.IndexOf(args, "-hash") > -1) hash = args[Array.IndexOf(args, "-hash") + 1];
            }
   
            Logger.LogInfoAction = Console.WriteLine;
            Logger.LogErrorAction = Console.WriteLine;
            Logger.LogWarningAction = Console.WriteLine;
       
            RoomApplication room = new RoomApplication(port);
            room.StartServer(); 
            room.InitRoomProcessWs(gateWsServerName, gateWsPort);
            DataMgr.Instance.BattleSession.StartupCFG.Update(mapId, port, playerNumber, gateWsServerName, gateWsPort, hash);

            //ConsoleLogger.CommandParser commandParser = new ConsoleLogger.CommandParser(logger);
            //commandParser.AddCommand(new ConsoleLogger.CommandAction("-exit", cmdParams => { AppQuit(); return ConsoleLogger.CommandExecuteRet.Break; }, "Exit app."))
            //    .ReadCommandLine();

            while (true)
                System.Threading.Thread.Sleep(1000);
        }



        static void AppQuit()
        {
            
        }

    }
}
