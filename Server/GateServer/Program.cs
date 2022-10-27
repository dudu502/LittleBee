using Net;
using ServerDll.Service;
using Service.Core;
using System;

namespace GateServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var pt = PtMessagePackage.Build(1203, null);
            var bytes = PtMessagePackage.Write(pt);
            var arry = new ArraySegment<byte>(bytes);
            var result = PtMessagePackage.Read(arry.ToArray());

            string logServerHost = "localhost:8001";
            string logUrl = $"http://{logServerHost}/log?content=";
          
            //string key = "Nuclear";//set in webserver.
            //int port = 9030;//set in webserver.
            ushort wsPort = 9000;
            Logger.LogInfoAction = Console.WriteLine;
            Logger.LogErrorAction = Console.WriteLine;
            Logger.LogWarningAction = Console.WriteLine;
            Logger.LogInfo("GATESERVER:"+ wsPort);
            Logger.LogInfo("LogServer Address: "+ logUrl);

            Services.GateApplication gate = new Services.GateApplication(wsPort);
            gate.AddConfigElement("log_server_address", logUrl);
            gate.StartServer();


            //ConsoleLogger.CommandParser commandParser = new ConsoleLogger.CommandParser(logger);
            //commandParser.AddCommand(new ConsoleLogger.CommandAction("-exit", (cmdParams) =>
            //{
            //    AppQuit();
            //    return ConsoleLogger.CommandExecuteRet.Break;
            //}, "Exit app."));
            //commandParser.ReadCommandLine();

            while (true)
                System.Threading.Thread.Sleep(1000);
        }


        private static void AppQuit()
        {
            //remove room process
        }
    }
}
