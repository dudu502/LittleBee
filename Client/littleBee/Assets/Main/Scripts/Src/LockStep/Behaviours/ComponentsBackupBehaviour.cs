using Net.Pt;
using Synchronize.Game.Lockstep.Ecsr.Entitas;
using Synchronize.Game.Lockstep.Proxy;
using Synchronize.Game.Lockstep.Room;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Synchronize.Game.Lockstep.Behaviours
{
    /// <summary>
    /// 组件内容备份
    /// </summary>
    public class ComponentsBackupBehaviour : ISimulativeBehaviour
    {
        bool EnableLog = false;
        bool EnableSnap = false;
        const int MaxSnapFileCount = 16;
        const int BackUpEntityWorldFrameLength = 128;
        const int SaveEntityWorldSnapshotFrameLength = BackUpEntityWorldFrameLength * 2;
        LogicFrameBehaviour logic;
        RoomServiceProxy roomServices;
        Dictionary<int, EntityWorldFrameData> m_DictEntityWorldFrameData;
        string _snapshotFolderPath = string.Empty;
        public Simulation Sim { set; get; }

        
        public ComponentsBackupBehaviour()
        {
            m_DictEntityWorldFrameData = new Dictionary<int, EntityWorldFrameData>();
        }
        public EntityWorldFrameData GetEntityWorldFrameByFrameIdx(int frameIdx)
        {
            if (m_DictEntityWorldFrameData.TryGetValue(frameIdx, out EntityWorldFrameData entityWorldFrameData))
                return entityWorldFrameData;
            return null;
        }

        public void SetEntityWorldFrameByFrameIdx(int frameIdx, EntityWorldFrameData data)
        {
            if (m_DictEntityWorldFrameData.TryGetValue(frameIdx, out EntityWorldFrameData entityWorldFrameData))
            {
                //entityWorldFrameData.Clear();
                m_DictEntityWorldFrameData.Remove(frameIdx);
            }
            m_DictEntityWorldFrameData[frameIdx] = data;
        }
        public Dictionary<int, EntityWorldFrameData> GetEntityWorldFrameData()
        {
            return m_DictEntityWorldFrameData;
        }

        public void Quit()
        {
            if (m_DictEntityWorldFrameData != null) m_DictEntityWorldFrameData.Clear();
            m_DictEntityWorldFrameData = null;
        }

        public void Start()
        {
            roomServices = DataProxy.Get<RoomServiceProxy>();
            logic = Sim.GetBehaviour<LogicFrameBehaviour>();
        }

        void SendKeyFrame(int idx)
        {
            PtKeyFrameCollection collection = roomServices.Session.GetKeyFrameCachedCollection();
            if (collection.KeyFrames.Count > 0)
            {
                roomServices.RequestSyncClientKeyframes(idx, collection);
                roomServices.Session.ClearKeyFrameCachedCollection();
            }
        }

        public void Update()
        {
            int frameIdx = logic.CurrentFrameIdx;
            SetEntityWorldFrameByFrameIdx(frameIdx, new EntityWorldFrameData(Sim.GetEntityWorld().GetAllCloneComponents()));
            SendKeyFrame(frameIdx);
            ClearEntityWorldFrameDataAt(frameIdx);
        }


        private void ClearEntityWorldFrameDataAt(int frameIdx)
        {
            int removeIdx = frameIdx - BackUpEntityWorldFrameLength;
            if (removeIdx >= 0)
            {
                if (m_DictEntityWorldFrameData.TryGetValue(removeIdx, out EntityWorldFrameData entityWorldFrameData))
                {
                    #region Enable Log
                    if (EnableLog)
                        SaveLogsToFile(removeIdx, entityWorldFrameData);
                    #endregion
                    #region Enable Snap
                    if (EnableSnap)
                        SaveEntityWorldSnapshotToFile(removeIdx, entityWorldFrameData);
                    #endregion
                    #region Clear FrameData
                    //entityWorldFrameData.Clear();
                    #endregion
                }
                m_DictEntityWorldFrameData.Remove(removeIdx);
            }
        }


        private async void SaveLogsToFile(int frameIdx, EntityWorldFrameData entityWorldFrameData)
        {
            string logFolderPath = BattleEntryPoint.PersistentDataPath + "/WorldLogs/" + roomServices.Session.RoomHash;
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }
            string entityWorldFrameDataStr = entityWorldFrameData.ToString();
            string sessionName = roomServices.Session.Name;
            await System.Threading.Tasks.Task.Run(() =>
            {
                bool skip = false;
                if (File.Exists(logFolderPath + "/" + sessionName + ".cache"))
                {
                    string idx = File.ReadAllText(logFolderPath + "/" + sessionName + ".cache");
                    if (int.TryParse(idx, out int cacheIndex))
                    {
                        skip = frameIdx <= cacheIndex;
                    }
                }

                if (!skip)
                {
                    File.WriteAllText(logFolderPath + "/" + sessionName + ".cache", frameIdx.ToString());
                    StreamWriter sw = File.AppendText(logFolderPath + "/" + sessionName + ".log");
                    sw.Write("FrameIdx:" + frameIdx + "\n" + entityWorldFrameDataStr);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            });

        }
        private string GetSnapshotFolderPath()
        {
            if(string.IsNullOrEmpty(_snapshotFolderPath))
                _snapshotFolderPath = BattleEntryPoint.PersistentDataPath + "/WorldSnapshots/" + roomServices.Session.RoomHash + "/" + roomServices.Session.Name;
            return _snapshotFolderPath;
        }
        private async void SaveEntityWorldSnapshotToFile(int frameIdx, EntityWorldFrameData entityWorldFrameData)
        {
            try
            {
                if (frameIdx > 0 && (frameIdx % SaveEntityWorldSnapshotFrameLength) == 0)
                {
                    string snapshotFolderPath = GetSnapshotFolderPath();
                    if (!Directory.Exists(snapshotFolderPath))
                        Directory.CreateDirectory(snapshotFolderPath);

                    byte[] binaryWorldFrameData = EntityWorldFrameData.Write(entityWorldFrameData);
                    await System.Threading.Tasks.Task.Run(() =>
                    {
                        using (Stream output = new FileStream($"{snapshotFolderPath}/{frameIdx}{Const.EXTENSION_TYPE_SNAP}", FileMode.OpenOrCreate))
                        {
                            output.Write(binaryWorldFrameData, 0, binaryWorldFrameData.Length);
                        }
                        // 删除多余的快照文件
                        string[] snapFiles = Directory.GetFiles(snapshotFolderPath, Const.EXTENSION_TYPE_PATTERN_SNAP);
                        List<int> fileNameSortList = new List<int>();
                        foreach (string fileName in snapFiles)
                        {
                            int fileNameToInt32 = Convert.ToInt32(System.IO.Path.GetFileNameWithoutExtension(fileName));
                            fileNameSortList.Add(fileNameToInt32);
                        }
                        fileNameSortList.Sort((a, b) => b - a);
                        while (fileNameSortList.Count > MaxSnapFileCount)
                        {
                            string snapshotFilePath = Path.Combine(snapshotFolderPath, fileNameSortList[fileNameSortList.Count - 1] + Const.EXTENSION_TYPE_SNAP);
                            if (File.Exists(snapshotFilePath))
                                File.Delete(snapshotFilePath);
                            fileNameSortList.RemoveAt(fileNameSortList.Count - 1);
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}
