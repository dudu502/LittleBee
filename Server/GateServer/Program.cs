
using Synchronize.Game.Lockstep.Service.Core;

namespace Synchronize.Game.Lockstep.GateServer
{
    class Program
    {      
        static void Main(string[] args)
        {
            string logServerHost = "localhost:8001";
            string logUrl = $"http://{logServerHost}/log?content=";
          
            string key = "Nuclear";//set in webserver.
            int port = 9030;//set in webserver.

            var logger = new ConsoleLogger.LoggerImpl.Logger("GATESERVER:"+port, logUrl);
            logger.EnableConsoleOutput = true;
            logger.Log("LogServer Address: "+ logUrl);
            BaseApplication.Logger = logger;
            Services.GateApplication gate = new Services.GateApplication(key);
            gate.AddConfigElement("log_server_address", logUrl);
            gate.StartServer(port);


            ConsoleLogger.CommandParser commandParser = new ConsoleLogger.CommandParser(logger);
            commandParser.AddCommand(new ConsoleLogger.CommandAction("-exit", (cmdParams) =>
            {
                AppQuit();
                return ConsoleLogger.CommandExecuteRet.Break;
            }, "Exit app."));
            commandParser.ReadCommandLine();

            //while (true)
            //    System.Threading.Thread.Sleep(1000);
        }


        private static void AppQuit()
        {
            //remove room process
        }
    }
}
