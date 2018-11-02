using System;
using System.Collections.Generic;
using XLua;

namespace BehaviorTree
{   
    public abstract class BTNode
    {
        /// <summary>
        /// 0 执行节点
        /// </summary>
        public const int NODE_TYPE_ACTION = 0;
        /// <summary>
        /// 1 顺序控制节点 （顺序执行）
        /// </summary>
        public const int NODE_TYPE_SEQUENCE = 1;
        /// <summary>
        /// 2 随机选择控制节点（子节点的weight字段来控制随机比例 选择一个节点执行之后结束）
        /// </summary>
        public const int NODE_TYPE_RANDOMSELECT = 2;
        /// <summary>
        /// 3 并发节点 （同时）
        /// </summary>
        public const int NODE_TYPE_PAIALLEL = 3;

        #region lua extends
        static LuaEnv m_LuaEnv = new LuaEnv();
        protected LuaTable m_LuaTable = null;

        protected Func<bool> m_DetectAction = null;       
        protected Action m_EnterAction = null;
        protected Action<float> m_UpdateAction = null;
        protected Action m_ExitAction = null;
        protected Action<string, object> m_TriggerAction = null;      
    
        #endregion

        #region members
        protected List<BTNode> m_Children = new List<BTNode>();
        protected BTNode m_Parent = null;
        protected BTNodeData m_Config = null;
        protected BTRoot m_Ai = null;
        protected bool m_BlActive = false;
        protected int m_IntChildWeight = 0;
        protected int m_ChildSeqIndex = 0;
        #endregion
        public Action<bool> OnExitHandler = null;

        public object UserData
        {
            get { return m_Ai.UserData; }           
        }

        
        protected bool HasNextChild()
        {
            return m_ChildSeqIndex < m_Children.Count-1;
        }
        protected BTNode GetNextChild()
        {
            ++m_ChildSeqIndex;
            return m_Children[m_ChildSeqIndex];
        }
        public bool Active
        {
            get { return m_BlActive; }
        }

        public BTNodeData Config
        {
            get { return m_Config; }
        }
        protected BTNode(BTRoot ai,BTNodeData config)
        {
            m_Config = config;
            m_Ai = ai;

            m_LuaTable = m_LuaEnv.NewTable();
            LuaTable meta = m_LuaEnv.NewTable();
            meta.Set("__index", m_LuaEnv.Global);
            m_LuaTable.SetMetaTable(meta);
            meta.Dispose();

            m_LuaTable.Set("self",this);
            m_LuaEnv.DoString(config.script, "AINode", m_LuaTable);
            m_LuaTable.Get("detect", out m_DetectAction);
            if(m_DetectAction == null)throw new System.Exception("must need function detect");
            m_LuaTable.Get("enter",out m_EnterAction);
            m_LuaTable.Get("update",out m_UpdateAction);
            m_LuaTable.Get("trigger", out m_TriggerAction);
            m_LuaTable.Get("exit",out m_ExitAction);           
        }
        
        public void SetParent(BTNode parent)
        {
            m_Parent = parent;        
        }
        public BTNode GetParent()
        {
            return m_Parent;
        }
        public List<BTNode> GetGenerations()
        {
            List<BTNode> gens = new List<BTNode>();
            BTNode node = this;
            do
            {
                gens.Add(node);
                node = node.m_Parent;
            } while (node != null);
            gens.Reverse();
            return gens;
        }

        public void Break()
        {       
            Exit(false);
            if (m_Parent != null)
            {
                switch (m_Parent.Config.type)
                {
                    case NODE_TYPE_SEQUENCE:
                        if (m_Parent.HasNextChild())
                            m_Parent.GetNextChild().Enter();
                        else
                            m_Parent.Break();
                        break;
                    case NODE_TYPE_RANDOMSELECT:
                        m_Parent.Break();
                        break;
                    case NODE_TYPE_PAIALLEL:
                        bool childAllActive = false;
                        m_Parent.m_Children.ForEach((c) => childAllActive |= c.Active);
                        if (!childAllActive)
                            m_Parent.Break();
                        break;
                    default:
                        m_Parent.Break();
                        throw new Exception("Need TODO");
                }
            }
        }

      
        /// <summary>
        /// Need Override
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objArray"></param>
        public virtual void TriggerFunc(string type, object obj)
        {
            
        }

        /// <summary>
        /// Need Override
        /// </summary>
        public virtual void Update(float dt)
        {
            
        }

        public BTNode GetRoot()
        {
            BTNode root = this;
            while (root.m_Parent != null)
                root = root.m_Parent;
            return root;
        }

        
        public bool IsRoot()
        {
            return GetRoot() == this;
        }

        public void Add(BTNode node)
        {
            m_Children.Add(node);
            node.SetParent(this);           
            m_IntChildWeight += node.Config.weight;
        }

        public bool Remove(BTNode node)
        {
            m_IntChildWeight -= node.Config.weight;
            return m_Children.Remove(node);         
        }

        
        public List<BTNode> GetChildren()
        {
            return m_Children;
        }

       
        /// <summary>
        /// Need Override
        /// </summary>
        public virtual void Enter()
        {
            m_BlActive = true;
        }

        public virtual void Exit(bool exitAll)
        {
            //if (!Active) return;
            if(exitAll)
            {
                foreach (BTNode child in m_Children)
                    child.Exit(true);
            }

            if (m_ExitAction != null)
                m_ExitAction();
            
            if (OnExitHandler != null)
                OnExitHandler(exitAll);
            m_BlActive = false;
            m_ChildSeqIndex = 0;
        }

        public virtual bool GetExecuting()
        {
            return false;
        }       
    }
}