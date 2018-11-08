using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class BTRoot
    {
        /// <summary>
        /// 树导出路径
        /// </summary>
        public const string TREE_OUTPUTPATH = "/Resources/BT/tree/";
        /// <summary>
        /// 脚本导出路径
        /// </summary>
        public const string SCRIPT_OUTPUTPATH = "/Resources/BT/txt/";
        /// <summary>
        /// 树结构数据
        /// </summary>
        public TextAsset m_TextDataAiTree;        

        /// <summary>
        /// 更新时间间隔（秒）
        /// </summary>
        public float m_UpdateIntervalSecs = 0.1f;
        public int Id;
        public bool m_BlLoop = true;
        float m_UpdatePassedSecs = 0;
        BTNode m_Root = null;
        List<BTNode> m_ListExecutingNodes = new List<BTNode>();
        bool m_BlAiActive = false;        
        public object UserData { get; set; }
        public BTRoot()
        {

        }
        public void Init()
        {
            m_Root = BTNodeData.Create(m_TextDataAiTree.bytes, this);
            m_Root.OnExitHandler = OnAiExitHandler;
        }
         
        void OnAiExitHandler(bool exitAll)
        {
            m_BlAiActive = false;
            if (!exitAll && m_BlLoop)
                StartAi();
        }    

        public void TriggerFunc(string type, object obj)
        {
            for (int i = m_ListExecutingNodes.Count - 1; i > -1; --i)
            {
                if (m_ListExecutingNodes[i] != null)
                    m_ListExecutingNodes[i].TriggerFunc(type, obj);
            }
        }


        public void StartAi()
        {
            m_BlAiActive = true;
            m_Root.Enter();    
        }

        public void StopAi()
        {
            m_BlAiActive = false;
            m_Root.Exit(true);
        }

        public void PauseAi()
        {
            m_BlAiActive = false;
        }

        public void ResumeAi()
        {
            m_BlAiActive = true;
        }

        public void Update(int ms)
        {
            /*
            m_UpdatePassedSecs += Time.deltaTime;
            if (m_UpdatePassedSecs >= m_UpdateIntervalSecs)
            {
                if (m_BlAiActive && m_ListExecutingNodes.Count > 0)
                {
                    for (int i = m_ListExecutingNodes.Count - 1; i > -1; --i)
                    {
                        if (m_ListExecutingNodes[i] != null)
                        {
                            m_ListExecutingNodes[i].Update(m_UpdatePassedSecs);
                        }
                    }
                }
                m_UpdatePassedSecs = 0;
            }*/
            if (m_BlAiActive && m_ListExecutingNodes.Count > 0)
            {
                for (int i = m_ListExecutingNodes.Count - 1; i > -1; --i)
                {
                    if (m_ListExecutingNodes[i] != null)
                    {
                        m_ListExecutingNodes[i].Update(ms/1000f);
                    }
                }
            }
        }
        public void SetExecutingNode(BTNode node)
        {
            if (!m_ListExecutingNodes.Contains(node))
                m_ListExecutingNodes.Add(node);              
        }
        public void RemoveExecutingNode(BTNode node)
        {
            if (m_ListExecutingNodes.Contains(node))
                m_ListExecutingNodes.Remove(node);
        }

        public List<BTNode> GetExecutingNodes()
        {
            return m_ListExecutingNodes;
        }

        public void Destroy()
        {
            m_ListExecutingNodes.Clear();
        }     
    }
}