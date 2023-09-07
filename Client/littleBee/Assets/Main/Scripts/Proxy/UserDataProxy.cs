using Synchronize.Game.Lockstep.Data;
using Synchronize.Game.Lockstep.Gate;
using Synchronize.Game.Lockstep.Managers;
using System.Net;

namespace Synchronize.Game.Lockstep.Proxy
{
    public class UserDataProxy : DataProxy
    {
        public string WebServerAddress { get { return UserSettingMgr.SettingList.Find((s) => s.m_SettingTitle == UserSettingMgr.INDEX_SERVER_ADDRESS).m_SettingValue; } }
        public LoginJsonResult UserLoginInfo;
        public string WanGateAddressIp { private set; get; } = string.Empty;
        public int WanGatePort { private set; get; } = 0;
        public string WanGateConnectKey { private set; get; } = string.Empty;
        protected override void OnInit()
        {
            base.OnInit();
        }
        public void SetWanGateInfo(string ip, int port, string connectKey)
        {
            WanGateAddressIp = ip;
            WanGatePort = port;
            WanGateConnectKey = connectKey;
        }
        public GateAddressVO GetGateAddressVO()
        {
            if (WanGateAddressIp == string.Empty || WanGatePort == 0 || WanGateConnectKey == string.Empty)
                return null;
            return new GateAddressVO() { RemotePoint = new System.Net.IPEndPoint(IPAddress.Parse(WanGateAddressIp), WanGatePort), ConnectKey = WanGateConnectKey, HashCode = 0 };
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
