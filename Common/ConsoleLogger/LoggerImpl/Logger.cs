using Net;
using Net.ServiceImpl;
using Service.Core;
using Service.HttpMisc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace ConsoleLogger.LoggerImpl
{
    public class Logger : ILogger
    {
        private object SyncObj = new object();
        public bool EnableConsoleOutput = true;
        private readonly string _Tag;
        private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";
        private string LogServerUrl;
        private int BufferSize;
        private Queue<string> _QueueLogs = new Queue<string>();
        public Logger(string tag,string logServerUrl,int bufferSize=16)
        {
            _Tag = tag;
            LogServerUrl = logServerUrl;
            BufferSize = bufferSize;
        }

        public void Log(string msg)
        {
            SendLogToLogServer($"[{DateTime.Now.ToString(_TimeFormatPatten)}]I[{_Tag}]\t{msg}");  
        }

        public void LogError(string msg)
        {
            SendLogToLogServer($"[{DateTime.Now.ToString(_TimeFormatPatten)}]E[{_Tag}]\t{msg}");
        }

        public void LogWarning(string msg)
        {
            SendLogToLogServer($"[{DateTime.Now.ToString(_TimeFormatPatten)}]W[{_Tag}]\t{msg}");
        }

        void SendLogToLogServer(string logMessage)
        {
            if (!string.IsNullOrEmpty(LogServerUrl))
            {
                lock(SyncObj)
                {
                    if (_QueueLogs.Count > BufferSize)
                    {
                        AsyncHttpTask.HttpGetRequest(LogServerUrl + string.Join("\n", _QueueLogs), null);
                        _QueueLogs.Clear();
                    }
                    else
                    {
                        _QueueLogs.Enqueue(logMessage);
                    }
                    
                }
            }
            if (EnableConsoleOutput)
            {
                Console.WriteLine(logMessage);
            }
        }
        
    }
}
