using Net.Pt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateServer.Core.Data
{
    public class DataMgr
    {
        #region singleInstance
        private static DataMgr _instance;
        public static DataMgr Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataMgr();
                return _instance;
            }
        }
        private DataMgr() { }
        #endregion

        public PtRoomList RoomList { private set; get; }
        public Dictionary<int, RoomProcess> ProcessIds = new Dictionary<int, RoomProcess>();
        public List<User> Users = new List<User>();
        public Dictionary<string, string> StartupConfigs = null;
        public int GateServerWSPort = 0;
        public void Init()
        {
            RoomList = new PtRoomList();
            RoomList.SetRooms(new List<PtRoom>());
        }
    }
}
