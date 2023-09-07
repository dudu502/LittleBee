using System.Collections;
using System.Threading;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Notification;
using Synchronize.Game.Lockstep.Proxy;

namespace Synchronize.Game.Lockstep.UI
{
    public class MainPanel : UIView
    {
        public Button m_BtnWan;
        public Button m_BtnReplays;
        public Button m_BtnSetting;
        public LoginAlert m_LoginAlert;

        public override void OnInit()
        {
            base.OnInit();
            m_BtnWan.onClick.AddListener(RequestIndex);
            m_BtnSetting.onClick.AddListener(OpenSettingPanel);
            m_BtnReplays.onClick.AddListener(OnClickReplays);
        }

        /// <summary>
        /// request webserver in wan mode
        /// to get gateserver ip and port.
        /// </summary>
        void RequestIndex()
        {
#if NONE_INDEX_SERVER
            m_LoginAlert.gameObject.SetActive(true);
            DataProxy.Get<UserDataProxy>().SetWanGateInfo("127.0.0.1", Convert.ToInt32("9030"), "Nuclear");
#else
        AsyncHttpTask.HttpGetRequest(DataProxy.Get<UserDataProxy>().WebServerAddress + "/get_index", (result_json) =>
        {
            if (result_json != "")
            {
                var obj = LitJson.JsonMapper.ToObject(result_json);
                print(obj["gs_addr"].ToString());
                print(obj["gs_port"].ToString());
                m_LoginAlert.gameObject.SetActive(true);
                DataProxy.Get<UserDataProxy>().SetWanGateInfo(obj["gs_addr"].ToString(), Convert.ToInt32(obj["gs_port"].ToString()), obj["gs_connect_key"].ToString());
            }
            else
            {
                // Evt.EventMgr<Tips.Alert, int>.TriggerEvent(Tips.Alert.Error, 10001);
                ToastRoot.Instance.ShowToast(Language.GetText(47));
            }
        },exception=> {
            ToastRoot.Instance.ShowToast(Language.GetText(48)+ exception.Message);
        });
#endif
        }
        void OpenSettingPanel()
        {
            ModuleManager.GetModule<UIModule>().Push(UITypes.SettingPanel, Layer.Bottom, null);
        }
        void OnClickReplays()
        {
            var replayFiles = Directory.GetFiles(BattleEntryPoint.PersistentDataPath, Const.EXTENSION_TYPE_PATTERN_REPLAY);
            if (replayFiles == null || replayFiles.Length == 0)
            {
                ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("No Replays"));
                return;
            }
            ModuleManager.GetModule<UIModule>().Push(UITypes.ReplaysPanel, Layer.Bottom, null);
        }
        public override void OnClose()
        {
            m_BtnReplays.onClick.RemoveAllListeners();
            m_BtnWan.onClick.RemoveAllListeners();
            m_BtnSetting.onClick.RemoveAllListeners();
            base.OnClose();
        }
    }
}