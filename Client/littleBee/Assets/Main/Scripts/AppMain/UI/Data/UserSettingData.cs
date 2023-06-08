using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
namespace Synchronize.Game.Lockstep.Managers 
{
    public class UserSettingMgr
    {
        public static readonly string LAN_SERVER_PORT = "LAN SERVER PORT: ";
        public static readonly string INDEX_SERVER_ADDRESS = "INDEX_SERVER_ADDRESS: ";
        public static List<UserSettingData> SettingList;
        public static void Init()
        {
            if (System.IO.File.Exists(Application.persistentDataPath + "\\setting.json"))
            {
                string json = System.IO.File.ReadAllText(Application.persistentDataPath + "\\setting.json");
                if (string.IsNullOrEmpty(json))
                {
                    InitDefaultSettingList();                    
                }
                else
                {
                    SettingList = JsonMapper.ToObject<List<UserSettingData>>(json);
                }
            }
            else
            {
                InitDefaultSettingList();
            }
        }

        static void InitDefaultSettingList()
        {
            SettingList = new List<UserSettingData>();
            SettingList.Add(new UserSettingData() { m_SettingTitle= INDEX_SERVER_ADDRESS ,m_SettingValue= "http://175.24.198.37:3000", m_ValueType= INPUT_STRING });
            SettingList.Add(new UserSettingData() { m_SettingTitle= LAN_SERVER_PORT ,m_SettingValue= "61001", m_ValueType=INPUT_NUMBER});
        }
        public static readonly byte INPUT_NUMBER = 0;
        public static readonly byte INPUT_STRING = 1;
    }

    public class UserSettingData
    {
       
        public byte m_Catalog;
        public string m_SettingTitle;
        public byte m_ValueType;
        public string m_SettingValue;
        public UserSettingData()
        {

        }
    }
}
