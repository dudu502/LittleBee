using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServer.Core.Data
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

        public BattleSession BattleSession { get; private set; }
        public void Init()
        {
            BattleSession = new BattleSession();
        }
    }
}
