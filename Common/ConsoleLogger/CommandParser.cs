using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleLogger
{
    public enum CommandExecuteRet
    { 
        Continue = 0,
        Break,
    }

    public class CommandAction
    {
        public string CommandKey;
        public Func<string[], CommandExecuteRet> CommandCaller;
        public string CommandDesc;
        public CommandAction(string key, Func<string[], CommandExecuteRet> func,string desc)
        {
            CommandKey = key;
            CommandCaller = func;
            CommandDesc = desc;
        }
        public override string ToString()
        {
            return $"{CommandKey}\t\t{CommandDesc}";
        }
    }

    public class CommandParser
    {
        ILogger Logger;
        List< CommandAction> _Commands;
        public CommandParser(ILogger logger)
        {
            Logger = logger;
            _Commands = new List< CommandAction>();
            _Commands.Add(new CommandAction("-help",(arr)=> {
                return Help();
            },"Help Info."));
    
        }
        CommandExecuteRet Help()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<Help>")
                .AppendLine(" [CommandKey]\t\t[Describe]");
            
            for (int i=0;i<_Commands.Count;++i)
            {
                stringBuilder.AppendLine($" [{i}]. "+_Commands[i].ToString());
            }
            Logger.Log(stringBuilder.ToString());
            return CommandExecuteRet.Continue;
        }
        public CommandParser AddCommand(CommandAction commandAction)
        {
            _Commands.Add(commandAction);
            return this;
        }
        public void ReadCommandLine()
        {
            for(; ; )
            {
                try
                {
                    string cmd = Console.ReadLine();
                    if (!string.IsNullOrEmpty(cmd))
                    {
                        string[] cmdParams = cmd.Split(' ');
                        if (cmdParams.Length > 0)
                        {
                            CommandAction commandAction = _Commands.Find((c) => c.CommandKey == cmdParams[0]);
                            if (commandAction != null)
                            {
                                CommandExecuteRet ret = commandAction.CommandCaller(cmdParams);
                                if (ret == CommandExecuteRet.Break)
                                    break;
                            }
                            else
                            {
                                Logger.LogError("Command Undefined." + cmd);
                            }
                        }
                    }
                }
                catch (Exception e) { }          
            }
        }
    }
}
