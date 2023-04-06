using System;
using System.Collections.Generic;
using System.Linq;

namespace Task.Switch.Structure.FSM
{
    public class StateMachine<TState, TParam> where TState : Enum where TParam : class
    {
        private enum StateMachineStatus
        {
            NotInitialized,
            Initialized,
            Running,
            Paused
        }
        public static Action<string> Log;
        private readonly List<State<TState, TParam>> m_States = new List<State<TState, TParam>>();
        private State<TState, TParam> m_CurrentActiveState = null;
        private StateMachineStatus m_Status = StateMachineStatus.Initialized;
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
            if (m_Status == StateMachineStatus.Initialized)
            {
                m_Status = StateMachineStatus.Running;
                m_CurrentActiveState = GetState(startStateName);
                m_CurrentActiveState.OnEnter();
            }
        }
        public void Stop()
        {
            m_Status = StateMachineStatus.NotInitialized;
            Log = null;
            m_States.Clear();
            m_CurrentActiveState = null;
        }
        public void Pause()
        {
            m_Status = StateMachineStatus.Paused;
        }
        public void Resume()
        {
            m_Status = StateMachineStatus.Running;
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
            m_Status = StateMachineStatus.Initialized;
            return this;
        }
        private State<TState, TParam> GetState(TState stateName)
        {
            foreach (State<TState, TParam> state in m_States)
                if (Enum.Equals(stateName, state.Name))
                    return state;
            throw new Exception($"{stateName} is not exist! Please call {nameof(NewState)} to create this state");
        }
        public void Update()
        {
            if (m_Status == StateMachineStatus.Running && m_CurrentActiveState != null)
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
