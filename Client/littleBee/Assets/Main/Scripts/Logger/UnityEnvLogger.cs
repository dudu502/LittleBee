using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronize.Game.Lockstep.Logger
{
    public class UnityEnvLogger : ILogger
    {
        private readonly string _Tag;
        private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";
        public UnityEnvLogger(string tag)
        {
            _Tag = tag;
        }
        public void Log(string msg)
        {
            UnityEngine.Debug.Log($"[{DateTime.Now.ToString(_TimeFormatPatten)}]I[{_Tag}]\t{msg}");
        }

        public void LogError(string msg)
        {
            UnityEngine.Debug.LogError($"[{DateTime.Now.ToString(_TimeFormatPatten)}]E[{_Tag}]\t{msg}");
        }

        public void LogWarning(string msg)
        {
            UnityEngine.Debug.LogWarning($"[{DateTime.Now.ToString(_TimeFormatPatten)}]W[{_Tag}]\t{msg}");
        }
    }
}
