using System.Collections;
using System.Threading;
using System.IO;
using Localization;
using Managers.UI;
using Proxy;
using Service.HttpMisc;
using UnityEngine;
using UnityEngine.UI;
using Misc;
using System;
using Managers;
using LogicFrameSync.Src.LockStep;
public class MainPanel : UIView, ILanguageApplicable
{
    public Button m_BtnWan;
    public Button m_BtnReplays;
    public Button m_BtnSetting;
    public LoginAlert m_LoginAlert;

    public override void OnInit()
    {
        base.OnInit();
        ApplyLocalizedLanguage();
        m_BtnWan.onClick.AddListener(RequestIndex);
        m_BtnSetting.onClick.AddListener(OpenSettingPanel);
        m_BtnReplays.onClick.AddListener(OnClickReplays);
    }
    public void ApplyLocalizedLanguage()
    {
        m_BtnWan.SetButtonText(Language.GetText(1));       
        m_BtnSetting.SetButtonText(Language.GetText(4));
        m_BtnReplays.SetButtonText(Language.GetText(25));
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
        if(replayFiles == null || replayFiles.Length == 0)
        {
            ToastRoot.Instance.ShowToast(Language.GetText(45));
            return;
        }
        ModuleManager.GetModule<UIModule>().Push(UITypes.ReplaysPanel,Layer.Bottom, null);
    }
    public override void OnClose()
    {
        m_BtnReplays.onClick.RemoveAllListeners();
        m_BtnWan.onClick.RemoveAllListeners();
        m_BtnSetting.onClick.RemoveAllListeners();
        base.OnClose();
    }
}
