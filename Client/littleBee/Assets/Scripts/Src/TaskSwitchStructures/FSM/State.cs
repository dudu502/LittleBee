using System;
using System.Collections.Generic;


namespace Task.Switch.Structure.FSM
{
    public class State<T> where T : Enum
    {
        public readonly List<Translation<T>> Translations = new List<Translation<T>>();
        private Action<uint> m_OnInitialize;
        private Action<uint> m_OnEnter;
        private Action<uint> m_OnUpdate;
        private Action<uint> m_OnExit;
        public readonly T Name;
        private readonly StateMachine<T> m_Machine;
        public State(T name, StateMachine<T> stateMachine)
        {
            Name = name; 
            m_Machine = stateMachine;
        }
        public uint GetEntityId()
        {
            return m_Machine.EntityId;
        }
        public StateMachine<T> End()
        {
            return m_Machine;
        }
        public Translation<T> Translate(Func<uint,bool> valid)
        {
            Translation<T> translation = new Translation<T>(this, valid);
            Translations.Add(translation);
            return translation;
        }
        public State<T> Initialize(Action<uint> init) { m_OnInitialize = init; return this; }
        public State<T> Enter(Action<uint> enter) { m_OnEnter = enter; return this; }
        public State<T> Update(Action<uint> update) { m_OnUpdate = update; return this; }
        public State<T> Exit(Action<uint> exit) { m_OnExit = exit; return this; }
        public void OnInitialize()
        {
            m_OnInitialize?.Invoke(m_Machine.EntityId);
        }
        public void OnEnter()
        {
            m_OnEnter?.Invoke(m_Machine.EntityId);
        }
        public void OnUpdate()
        {
            m_OnUpdate?.Invoke(m_Machine.EntityId);
        }
        public void OnExit()
        {
            m_OnExit?.Invoke(m_Machine.EntityId);
        }

    }
}
