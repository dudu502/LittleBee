using MappingData;
using UI.Data;

namespace Proxy
{
    public class UserDataProxy : DataProxy
    {
        public string WebServerAddress { get { return UserSettingMgr.SettingList.Find((s) => s.m_SettingTitle == UserSettingMgr.INDEX_SERVER_ADDRESS).m_SettingValue; } }
        public LoginJsonResult UserLoginInfo;
        public string GateWS { get; private set; }
        public string GateName { get; private set; }
        protected override void OnInit()
        {
            base.OnInit();
        }

        public void SetWanGateInfo(string gateWs,string gateName)
        {
            GateWS = gateWs;
            GateName = gateName;
        }
        
        public string GetUserName()
        {
            if (!UserLoginInfo.IsEmpty())
                return UserLoginInfo.results[0].name;
            return "";
        }
        public string GetUserId()
        {
            return UserLoginInfo.results[0].GetId();
        }
    }
}
