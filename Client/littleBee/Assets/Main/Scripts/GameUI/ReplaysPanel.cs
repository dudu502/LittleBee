using System.Security.AccessControl;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Net.Pt;
using System.IO;
using System.Threading.Tasks;
using Synchronize.Game.Lockstep.Managers.UI;
using Synchronize.Game.Lockstep.Localization;
using Synchronize.Game.Lockstep.Managers;
using Synchronize.Game.Lockstep.Misc;
using Synchronize.Game.Lockstep.Notification;

namespace Synchronize.Game.Lockstep.UI
{
    public class ReplaysPanel : UIView
    {
        public TMPro.TMP_Text m_TxtTitle;
        public Button m_BtnBack;
        public Button m_BtnPlayReplay;
        public DynamicInfinityListRenderer m_DlReplaysListRender;
        public TMPro.TMP_InputField m_InputRename;
        public Button m_BtnRename;

        public TMPro.TMP_Text m_TxtSize;

        public TMPro.TMP_Text m_TxtLength;

        public override void OnInit()
        {
            base.OnInit();
            m_BtnBack.onClick.AddListener(OnClickBack);
            m_BtnPlayReplay.onClick.AddListener(OnClickPlayReplay);
            m_DlReplaysListRender.InitRendererList(null, null, OnReplayItemEvent);
            m_DlReplaysListRender.SetDataProvider(new List<ReplayItemRenderer.ReplayItemData>());
            m_BtnRename.onClick.AddListener(OnClickRename);
            LoadReplays();
        }
        override public void OnResume()
        {
            base.OnResume();
        }
        void LoadReplays()
        {
            var replayFiles = Directory.GetFiles(BattleEntryPoint.PersistentDataPath, Const.EXTENSION_TYPE_PATTERN_REPLAY);

            foreach (string replayFile in replayFiles)
            {
                ReplayItemRenderer.ReplayItemData replayItemData = new ReplayItemRenderer.ReplayItemData(replayFile);
                m_DlReplaysListRender.GetDataProvider().Add(replayItemData);
            }

           
            if (m_DlReplaysListRender.GetDataProvider().Count > 0)
            {
                ((ReplayItemRenderer.ReplayItemData)m_DlReplaysListRender.GetDataProvider()[0]).IsSelect = true;
                OnReplayItemEvent(new DynamicInfinityItem.Event("Select", ((ReplayItemRenderer.ReplayItemData)m_DlReplaysListRender.GetDataProvider()[0])));
            }
            m_DlReplaysListRender.RefreshDataProvider();
        }

        void OnReplayItemEvent(DynamicInfinityItem.Event evt)
        {
            if (evt.EventName == "Select")
            {
                foreach (var item in m_DlReplaysListRender.GetDataProvider())
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
            if (replayItemData != null)
            {
                var replay = await replayItemData.GetReplayInfoAsync();
                m_InputRename.text = replayItemData.GetFileNameWithoutExtension();
                m_TxtSize.text = $"{Mathf.CeilToInt(replayItemData.RepFileInfo.Length / 1024f)} KB";
                m_TxtLength.text = $"{(replay.Frames.Count * 0.04f).ToString()} S";
            }
        }
        void OnClickRename()
        {
            string fileName = m_InputRename.text;
            if (string.IsNullOrEmpty(fileName))
            {
                ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("Name cannot be empty"));
            }
            else
            {
                foreach (ReplayItemRenderer.ReplayItemData item in m_DlReplaysListRender.GetDataProvider())
                {
                    if (item.GetFileNameWithoutExtension() == fileName)
                    {
                        ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("Please try again"));
                        return;
                    }
                }
                foreach (ReplayItemRenderer.ReplayItemData data in m_DlReplaysListRender.GetDataProvider())
                {
                    if (data.IsSelect)
                    {
                        string newPathName = Path.Combine(Path.GetDirectoryName(data.ReplayFileFullPath), fileName + Const.EXTENSION_TYPE_REPLAY);
                        File.Move(data.ReplayFileFullPath, newPathName);
                        data.ReplayFileFullPath = newPathName;
                        ToastManager.Instance.ShowToast(Localization.Localization.GetTranslation("Rename succeeded"));
                        break;
                    }
                }
                m_DlReplaysListRender.RefreshDataProvider();
            }
        }

        async void OnClickPlayReplay()
        {
            foreach (var item in m_DlReplaysListRender.GetDataProvider())
            {
                if (((ReplayItemRenderer.ReplayItemData)item).IsSelect)
                {
                    Simulation replaySim = BattleEntryPoint.CreateReplaySimulation();
                    BattleEntryPoint.StartReplay(replaySim, await ((ReplayItemRenderer.ReplayItemData)item).GetReplayInfoAsync());
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
    }
}