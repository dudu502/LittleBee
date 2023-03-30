using System;
using System.Collections.Generic;

namespace Task.Switch.Structure.FSM
{
    public class StateMachine<TState, TParam> where TState : Enum where TParam : class
    {
        public static Action<string> Log;
        private readonly List<State<TState, TParam>> m_States = new List<State<TState, TParam>>();
        private State<TState, TParam> m_CurrentActiveState = null;
        private bool m_Running = false;
        private bool m_Inited = false;
        private readonly TParam m_Parameter;
        public StateMachine(TParam param)
        {
            m_Parameter = param;
        }

        internal TParam GetParameter()
        {
            return m_Parameter;
        }

        public void Start(TState startStateName)
        {
            if (m_Inited && !m_Running)
            {
                m_Running = true;
                m_CurrentActiveState = GetState(startStateName);
                m_CurrentActiveState.OnEnter();
            }
        }
        public void Stop()
        {
            m_Running = false;
            m_Inited = false;
            Log = null;
            m_States.Clear();
            m_CurrentActiveState = null;
        }
        public void Pause()
        {
            m_Running = false;
        }
        public void Resume()
        {
            m_Running = true;
        }
        public State<TState, TParam> NewState(TState stateName)
        {
            State<TState, TParam> state = new State<TState, TParam>(stateName, this);
            m_States.Add(state);
            return state;
        }
        public StateMachine<TState, TParam> Any(TState to, Func<TParam, bool> valid, Action<TParam> transfer = null)
        {
            foreach (State<TState, TParam> state in m_States)
            {
                if (!Enum.Equals(to, state.Name))
                {
                    Translation<TState, TParam> translation = new Translation<TState, TParam>(state, valid).Transfer(transfer);
                    translation.To(to);
                    state.Translations.Add(translation);
                }
            }
            return this;
        }
        public StateMachine<TState, TParam> Initialize()
        {
            foreach (State<TState, TParam> state in m_States)
                state.OnInitialize();
            m_Inited = true;
            return this;
        }
        private State<TState, TParam> GetState(TState stateName)
        {
            foreach (State<TState, TParam> state in m_States)
            {
                if (Enum.Equals(state.Name, stateName))
                    return state;
            }
            throw new Exception($"{stateName} is not exist! Please call {nameof(NewState)} to create this state");
        }
        public void Update()
        {
            if (m_Running && m_CurrentActiveState != null)
            {
                foreach (Translation<TState, TParam> translation in m_CurrentActiveState.Translations)
                {
                    if (translation.OnValid())
                    {
                        m_CurrentActiveState.OnExit();
                        m_CurrentActiveState = GetState(translation.ToStateName);
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
