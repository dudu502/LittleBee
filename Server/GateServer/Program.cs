using GateServer.Modules;
using Service.Core;
using Service.HttpMisc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GateServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Console.WriteLine("Please Input LogServer Host (input -d as default [localhost:8001]):");
            //string logServerHost = Console.ReadLine();
            //if (logServerHost == "-d")
            string logServerHost = "localhost:8001";
            string logUrl = $"http://{logServerHost}/log?content=";
          
            string key = "Nuclear";//set in webserver.
            int port = 9030;//set in webserver.

            var logger = new ConsoleLogger.LoggerImpl.Logger("GATESERVER:"+port, logUrl);
            logger.EnableConsoleOutput = false;
            logger.Log("LogServer Address: "+ logUrl);
            BaseApplication.Logger = logger;
            Services.GateApplication gate = new Services.GateApplication(key);
            gate.AddConfigElement("log_server_address", logUrl);
            gate.StartServer(port);


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
