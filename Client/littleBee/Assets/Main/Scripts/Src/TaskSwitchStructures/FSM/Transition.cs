using System;
namespace Synchronize.Game.Lockstep.FSM
{
    public class Transition<TState, TParam> where TState : Enum where TParam : class
    {
        private Func<TParam, bool> m_Valid;
        private Action<TParam> m_Transfer;
        internal TState ToStateName { private set; get; }
        private readonly State<TState, TParam> m_Current;
        public Transition(State<TState, TParam> state, Func<TParam, bool> valid)
        {
            m_Current = state;
            m_Valid = valid;
        }
        public Transition(State<TState, TParam> state)
        {
            m_Current = state;
        }

        public static Transition<TState,TParam> Clone(Transition<TState,TParam> origin, State<TState,TParam> state)
        {
            Transition<TState, TParam> clone = new Transition<TState, TParam>(state);
            clone.m_Valid = origin.m_Valid;
            clone.m_Transfer = origin.m_Transfer;
            clone.ToStateName = origin.ToStateName;
            return clone;
        }
        public State<TState, TParam> To(TState stateName)
        {
            ToStateName = stateName;
            return m_Current;
        }
        internal bool OnValid()
        {
            bool valid = false;
            if (m_Valid != null)
                valid = m_Valid(m_Current.GetParameter());
            if (StateMachine<TState, TParam>.Log != null)
                StateMachine<TState, TParam>.Log($"State:{m_Current.Name} {nameof(OnValid)}:{valid} ToState:{ToStateName}");
            return valid;
        }
        public Transition<TState, TParam> Transfer(Action<TParam> transfer)
        {
            m_Transfer = transfer;
            return this;
        }
        internal void OnTransfer()
        {
            if (m_Transfer != null)
            {
                if (StateMachine<TState, TParam>.Log != null)
                    StateMachine<TState, TParam>.Log($"State:{m_Current.Name} {nameof(OnTransfer)} ToState:{ToStateName}");
                m_Transfer(m_Current.GetParameter());
            }
        }
    }
}
