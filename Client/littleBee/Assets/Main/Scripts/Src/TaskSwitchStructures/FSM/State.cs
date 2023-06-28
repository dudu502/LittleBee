using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.FSM
{
    public class State<TState, TParam> where TState : Enum where TParam : class
    {
        internal readonly TState Name;
        readonly StateMachine<TState, TParam> m_Machine;
        internal readonly List<Transition<TState, TParam>> Transitions = new List<Transition<TState, TParam>>();
        private Action<TParam> m_OnInitialize;
        private Action<TParam> m_OnEnter;
        private Action<TParam> m_OnUpdate;
        private Action<TParam> m_OnExit;


        internal static State<TState,TParam> Clone(State<TState,TParam> origin, StateMachine<TState,TParam> stateMachine)
        {
            State<TState, TParam> clone = new State<TState, TParam>(origin.Name, stateMachine);
            clone.m_OnInitialize = origin.m_OnInitialize;
            clone.m_OnEnter = origin.m_OnEnter;
            clone.m_OnUpdate= origin.m_OnUpdate;
            clone.m_OnExit = origin.m_OnExit;

            foreach(Transition<TState,TParam> transition in origin.Transitions)
                clone.Transitions.Add(Transition<TState, TParam>.Clone(transition,clone));

            return clone;
        }

        public State(TState name, StateMachine<TState, TParam> stateMachine)
        {
            Name = name;
            m_Machine = stateMachine;
        }

        internal TParam GetParameter()
        {
            return m_Machine.GetParameter();
        }
        public StateMachine<TState, TParam> End()
        {
            return m_Machine;
        }
        public Transition<TState, TParam> When(Func<TParam, bool> valid)
        {
            Transition<TState, TParam> transition = new Transition<TState, TParam>(this, valid);
            Transitions.Add(transition);
            return transition;
        }
        public State<TState, TParam> Initialize(Action<TParam> init) { m_OnInitialize = init; return this; }
        public State<TState, TParam> Enter(Action<TParam> enter) { m_OnEnter = enter; return this; }
        public State<TState, TParam> Update(Action<TParam> update) { m_OnUpdate = update; return this; }
        public State<TState, TParam> Exit(Action<TParam> exit) { m_OnExit = exit; return this; }
        internal void OnInitialize()
        {
            if (m_OnInitialize != null)
            {
                if (StateMachine<TState, TParam>.Log != null)
                    StateMachine<TState, TParam>.Log($"{Name} {nameof(OnInitialize)}");
                m_OnInitialize(GetParameter());
            }
        }
        internal void OnEnter()
        {
            if (m_OnEnter != null)
            {
                if (StateMachine<TState, TParam>.Log != null)
                    StateMachine<TState, TParam>.Log($"{Name} {nameof(OnEnter)}");
                m_OnEnter(GetParameter());
            }
        }

        internal void OnUpdate()
        {
            if (m_OnUpdate != null)
            {
                if (StateMachine<TState, TParam>.Log != null)
                    StateMachine<TState, TParam>.Log($"{Name} {nameof(OnUpdate)}");
                m_OnUpdate(GetParameter());
            }
        }
        internal void OnExit()
        {
            if (m_OnExit != null)
            {
                if (StateMachine<TState, TParam>.Log != null)
                    StateMachine<TState, TParam>.Log($"{Name} {nameof(OnExit)}");
                m_OnExit(GetParameter());
            }
        }
    }
}
