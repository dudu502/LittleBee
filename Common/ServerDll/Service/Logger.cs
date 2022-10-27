using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDll.Service
{
    public class Logger
    {
        public static Action<string> LogInfoAction;
        public static void LogInfo(string value)
        {
            LogInfoAction?.Invoke(value);
        }

        public static Action<string> LogErrorAction;
        public static void LogError(string value)
        {
            LogErrorAction?.Invoke(value);
        }
        public static Action<string> LogWarningAction;
        public static void LogWarning(string value)
        {
            LogWarningAction?.Invoke(value);
        }
    }
}
