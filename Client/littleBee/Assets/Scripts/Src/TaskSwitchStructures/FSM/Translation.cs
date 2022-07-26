using System;

namespace Task.Switch.Structure.FSM
{
    public class Translation<T> where T:Enum
    {
        private readonly Func<uint,bool> m_Valid;
        private Action m_Transfer;
        public T ToStateName { private set; get; }
        private readonly State<T> m_Current;
        public Translation(State<T> state, Func<uint,bool> valid)
        {
            m_Current = state;
            m_Valid = valid;
        }
        public State<T> To(T stateName)
        {
            ToStateName = stateName;
            return m_Current;
        }
        public bool OnValid()
        {
            if (m_Valid != null)
                return m_Valid(m_Current.GetEntityId());
            return false;
        }
        public Translation<T> Transfer(Action transfer)
        {
            m_Transfer = transfer;
            return this;
        }
        public void OnTransfer()
        {
            m_Transfer?.Invoke();
        }
    }
}
