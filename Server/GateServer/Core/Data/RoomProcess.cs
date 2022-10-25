using Net.Pt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateServer.Core.Data
{
    public class RoomProcess
    {
        public Process CurrentProcess { private set; get; }
        public uint RoomId { set; get; }
        public int Port { set; get; }
        public PtLaunchGameData LaunchGameData { set; get; }
        public void Set(Process proc)
        {
            CurrentProcess = proc;
        }
        public void Kill()
        {
            if (CurrentProcess != null)
                CurrentProcess.Kill();
        }
    }
}
