using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Synchronize.Game.Lockstep.Replays;

namespace Synchronize.Game.Lockstep.UI
{
    public class ReplayItemRenderer : DynamicInfinityItem
    {
        const string _timeFormatPatten = "yyyy/MM/dd HH:mm:ss";
        public class ReplayItemData
        {

            public string ReplayFileFullPath;
            public bool IsSelect;

            private byte[] replayBinaryBytes;

            private ReplayInfo replayInfo;

            public async Task<ReplayInfo> GetReplayInfoAsync()
            {
                if (replayInfo == null)
                {
                    if (replayBinaryBytes == null)
                        replayBinaryBytes = await System.Threading.Tasks.Task.Run(() => File.ReadAllBytes(ReplayFileFullPath));
                    replayInfo = await ReplayInfo.Read(replayBinaryBytes);
                }
                return replayInfo;
            }
            public FileInfo RepFileInfo;
            public ReplayItemData(string fullPath)
            {
                ReplayFileFullPath = fullPath;
            }
            public string GetFileNameWithoutExtension()
            {
                return System.IO.Path.GetFileNameWithoutExtension(ReplayFileFullPath);
            }
        }
        public Image m_ImgBg;
        public TMPro.TMP_Text m_TxtReplayName;
        public TMPro.TMP_Text m_TxtReplayCreationDate;
        void Start()
        {
            EventTriggerListener.Get(m_ImgBg.gameObject).onClick += g =>
            {
                if (OnEventHandler != null)
                    OnEventHandler(new Event("Select", GetData()));
            };
        }
        protected override void OnRenderer()
        {
            ReplayItemData replayData = GetData<ReplayItemData>();
            m_ImgBg.color = replayData.IsSelect ? new UnityEngine.Color(100 / 255f, 200 / 255f, 1) : UnityEngine.Color.white;
            m_TxtReplayName.text = replayData.GetFileNameWithoutExtension();
            if (replayData.RepFileInfo == null)
                replayData.RepFileInfo = new FileInfo(replayData.ReplayFileFullPath);
            m_TxtReplayCreationDate.text = replayData.RepFileInfo.LastWriteTime.ToString(_timeFormatPatten);
        }
    }
}