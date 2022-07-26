using System;
using System.Collections.Generic;

namespace Task.Switch.Structure.FSM
{
    public class StateMachine<T> where T : Enum
    {
        private readonly List<State<T>> m_States = new List<State<T>>();
        private State<T> m_CurrentActiveState = null;
        private bool m_Running = false;
        private bool m_Inited = false;
        public uint EntityId { private set; get; }
        public StateMachine(uint enitiyId)
        {
            EntityId = enitiyId; 
        }
        public void Start(T startStateName)
        {
            if(m_Inited)
            {
                m_Running = true;
                m_CurrentActiveState = Find(startStateName);
                m_CurrentActiveState.OnEnter();
            }
        }
        public void Stop()
        {
            m_Running=false;
        }

        public State<T> NewState(T stateName)
        {
            State<T> state = new State<T>(stateName, this);
            m_States.Add(state);
            return state;
        }
        public StateMachine<T> Initialize()
        {
            foreach (State<T> state in m_States)
                state.OnInitialize();
            m_Inited = true;
            return this;
        }
        private State<T> Find(T stateName)
        {
            foreach (State<T> state in m_States)
            {
                if (Enum.Equals(state.Name, stateName))
                    return state;
            }
            throw new Exception($"{stateName} is not exist! Please call NewState to create this state");
        }
        public void Update()
        {
            if (m_Running && m_CurrentActiveState != null)
            {
                foreach (Translation<T> translation in m_CurrentActiveState.Translations)
                {
                    if (translation.OnValid())
                    {
                        m_CurrentActiveState.OnExit();
                        m_CurrentActiveState = Find(translation.ToStateName);
                        translation.OnTransfer();
                        m_CurrentActiveState.OnEnter();
                        return;
                    }
                }
                m_CurrentActiveState.OnUpdate();
            }
        }
    }
}
