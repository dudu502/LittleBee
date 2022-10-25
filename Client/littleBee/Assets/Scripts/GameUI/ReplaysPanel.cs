using System.Security.AccessControl;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Managers.UI;
using Localization;
using Misc;
using UnityEngine.UI;
using Proxy;
using NetServiceImpl;
using Net.Pt;
using Managers;
using Managers.Config;
using System.IO;
using LogicFrameSync.Src.LockStep;

using System.Threading.Tasks;

public class ReplaysPanel : UIView, ILanguageApplicable
{
    public Text m_TxtTitle;
    public Button m_BtnBack;
    public Button m_BtnPlayReplay;
    public DynamicInfinityListRenderer m_DlReplaysListRender;
    public InputField m_InputRename;
    public Text m_TxtRenameLabel;
    public Button m_BtnRename;

    public Text m_TxtSizeLabel;
    public Text m_TxtSize;

    public Text m_TxtLengthLabel;
    public Text m_TxtLength;
    public override void OnInit()
    {
        base.OnInit();
        m_BtnBack.onClick.AddListener(OnClickBack);
        m_BtnPlayReplay.onClick.AddListener(OnClickPlayReplay);
        m_DlReplaysListRender.InitRendererList(null, null,OnReplayItemEvent);
        m_BtnRename.onClick.AddListener(OnClickRename);
        LoadReplays();
        ApplyLocalizedLanguage();
    }
    override public void OnResume()
    {
        base.OnResume();        
    }
    void LoadReplays()
    {
        var replayFiles = Directory.GetFiles( BattleEntryPoint.PersistentDataPath,Const.EXTENSION_TYPE_PATTERN_REPLAY);
        List<ReplayItemRenderer.ReplayItemData> replayItemDatas = new List<ReplayItemRenderer.ReplayItemData>();
        foreach (string replayFile in replayFiles)
        {
            ReplayItemRenderer.ReplayItemData replayItemData = new ReplayItemRenderer.ReplayItemData(replayFile);
            replayItemDatas.Add(replayItemData);
        }
     
        m_DlReplaysListRender.SetDataProvider(replayItemDatas);
        if (replayItemDatas.Count > 0)
        {
            replayItemDatas[0].IsSelect = true;
            OnReplayItemEvent(new DynamicInfinityItem.Event("Select", replayItemDatas[0]));
        }
    }

    void OnReplayItemEvent(DynamicInfinityItem.Event evt)
    {
        if (evt.EventName == "Select")
        {
            foreach(var item in m_DlReplaysListRender.GetDataProvider())
            {
                ((ReplayItemRenderer.ReplayItemData)item).IsSelect = item == evt.Data;
            }
        }
        m_DlReplaysListRender.RefreshDataProvider();
        // show replay information
        ShowReplayInfo(evt.Data as ReplayItemRenderer.ReplayItemData);
    }
    async void ShowReplayInfo(ReplayItemRenderer.ReplayItemData replayItemData)
    {
        if(replayItemData!=null)
        {
            var replay = await replayItemData.GetReplayInfoAsync();
            m_InputRename.text = replayItemData.GetFileNameWithoutExtension();
            m_TxtSize.text = $"{Mathf.CeilToInt(replayItemData.RepFileInfo.Length/1024f)} KB";
            m_TxtLength.text = $"{(replay.Frames.Count * 0.04f).ToString()} S";
        }
    }
    void OnClickRename()
    {
        string fileName = m_InputRename.text;
        if(string.IsNullOrEmpty(fileName))
        {
            ToastRoot.Instance.ShowToast(Language.GetText(46));
        }
        else
        {
            foreach(ReplayItemRenderer.ReplayItemData item in m_DlReplaysListRender.GetDataProvider())
            {
                if(item.GetFileNameWithoutExtension() == fileName)
                {
                    ToastRoot.Instance.ShowToast(Language.GetText(42));
                    return;
                }               
            }
            foreach (ReplayItemRenderer.ReplayItemData data in m_DlReplaysListRender.GetDataProvider())
            {
                if(data.IsSelect)
                {
                    string newPathName = Path.Combine(Path.GetDirectoryName(data.ReplayFileFullPath), fileName + Const.EXTENSION_TYPE_REPLAY);
                    File.Move(data.ReplayFileFullPath, newPathName);
                    data.ReplayFileFullPath = newPathName;
                    ToastRoot.Instance.ShowToast(Language.GetText(43));
                    break;
                }
            }
            m_DlReplaysListRender.RefreshDataProvider();
        }
    }
   
    async void OnClickPlayReplay()
    {
        foreach(var item in m_DlReplaysListRender.GetDataProvider())
        {
            if(((ReplayItemRenderer.ReplayItemData)item).IsSelect)
            {
                Simulation replaySim = BattleEntryPoint.CreateReplaySimulation();
                BattleEntryPoint.StartReplay(replaySim,await ((ReplayItemRenderer.ReplayItemData)item).GetReplayInfoAsync());
            }
        }     
    }

    void OnClickBack()
    {       
        ModuleManager.GetModule<UIModule>().Pop(Layer.Bottom); 
    }
    public override void OnClose()
    {
        base.OnClose();
        m_BtnBack.onClick.RemoveAllListeners();
        m_BtnPlayReplay.onClick.RemoveAllListeners();
        m_InputRename.onValueChanged.RemoveAllListeners();
        m_BtnRename.onClick.RemoveAllListeners();

    }
    public void ApplyLocalizedLanguage()
    {
        m_TxtTitle.text = Language.GetText(25);
        m_BtnBack.SetButtonText(Language.GetText(5));
        m_BtnPlayReplay.SetButtonText(Language.GetText(24));
        m_TxtRenameLabel.text = Language.GetText(33);
        m_TxtSizeLabel.text = Language.GetText(34);
        m_TxtLengthLabel.text = Language.GetText(35);
    }
}