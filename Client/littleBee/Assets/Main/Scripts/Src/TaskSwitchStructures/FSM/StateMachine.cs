
using System;
using System.Collections.Generic;
using System.Linq;

namespace Synchronize.Game.Lockstep.FSM
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
        private readonly List<State<TState, TParam>> m_States;
        private State<TState, TParam> m_CurrentActiveState = null;
        private StateMachineStatus m_Status = StateMachineStatus.NotInitialized;
        private readonly TParam m_Parameter;
        public StateMachine(TParam param)
        {
            m_Parameter = param;
            m_States = new List<State<TState,TParam>>();
        }

        public static StateMachine<TState, TParam> Clone(StateMachine<TState,TParam> original, TParam param) 
        {
            StateMachine<TState, TParam> clone = new StateMachine<TState, TParam>(param);
            foreach (State<TState, TParam> state in original.m_States)
                clone.m_States.Add(State<TState, TParam>.Clone(state, clone));
            return clone;
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
                    Transition<TState, TParam> transition = new Transition<TState, TParam>(state, valid).Transfer(transfer);
                    transition.To(to);
                    state.Transitions.Add(transition);
                }
            }
            return this;
        }
        public StateMachine<TState, TParam> Where(TState from, TState to, Func<TParam, bool> valid, Action<TParam> transfer = null)
        {
            foreach (State<TState, TParam> state in m_States)
            {
                if ((from.GetHashCode() & state.Name.GetHashCode()) == state.Name.GetHashCode())
                {
                    Transition<TState, TParam> transition = new Transition<TState, TParam>(state, valid).Transfer(transfer);
                    transition.To(to);
                    state.Transitions.Add(transition);
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
            foreach(State<TState,TParam> state in m_States)
                if (Enum.Equals(stateName, state.Name))
                    return state;
            throw new Exception($"{stateName} is not exist! Please call {nameof(NewState)} to create this state");
        }
        public void Update()
        {
            if (m_Status == StateMachineStatus.Running && m_CurrentActiveState != null)
            {
                foreach (Transition<TState, TParam> transition in m_CurrentActiveState.Transitions)
                {
                    if (transition.OnValid())
                    {
                        m_CurrentActiveState.OnExit();
                        m_CurrentActiveState = GetState(transition.ToStateName);
                        transition.OnTransfer();
                        m_CurrentActiveState.OnEnter();
                        return;
                    }
                }
                m_CurrentActiveState.OnUpdate();
            }
        }
    }
}
