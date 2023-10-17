using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.FSM
{
    public interface IStateMachine<TObject>
    {
        void SetParameter<TObjecct>(TObject param);
        TObject GetParameter();
        void Reset();
        IState<TObject> State<TState>(TState id);
        IStateMachine<TObject> SetDefault<TState>(TState id);
        void Update();
        IStateMachine<TObject> Build();
        IStateMachine<TObject> Any<TState>(Func<TObject, bool> valid, TState toId, Action<TObject> transfer);
        IStateMachine<TObject> Select<TState>(Func<TObject, bool> valid, TState id, TState toId, Action<TObject> transfer);
    }

    public interface IState<TObject>
    {
        IState<TObject> Initialize(Action<TObject> onInit);
        IState<TObject> Enter(Action<TObject> onEnter);
        IState<TObject> EarlyUpdate(Action<TObject> onEarlyUpdate);
        IState<TObject> Update(Action<TObject> onUpdate);
        IState<TObject> Exit(Action<TObject> onExit);
        IStateMachine<TObject> End();
        ITransition<TObject> Transition(Func<TObject, bool> valid);
    }

    public interface ITransition<TObject>
    {
        ITransition<TObject> Transfer(Action<TObject> onTransfer);
        ITransition<TObject> To<TState>(TState id);
        ITransition<TObject> ToEnd();
        ITransition<TObject> ToEntry();
        IState<TObject> End();
    }

    internal class Transition<TObject>
    {
        public int Id { get; private set; }
        public int ToId { get; set; }
        private Func<TObject, bool> m_Validate;
        private Action<TObject> m_Transfer;

        public Transition(int id, int toId, Func<TObject, bool> valid, Action<TObject> transfer)
        {
            Id = id;
            ToId = toId;
            m_Validate = valid;
            m_Transfer = transfer;
        }

        public Transition(int id, Func<TObject, bool> valid)
        {
            Id = id;
            m_Validate = valid;
        }

        public void SetTransfer(Action<TObject> transfer)
        {
            m_Transfer = transfer;
        }

        public Action<TObject> GetTransfer()
        {
            return m_Transfer;
        }

        public void OnTransfer(TObject param)
        {
            if (m_Transfer != null)
                m_Transfer(param);
        }

        public bool OnValidate(TObject param)
        {
            bool validate = false;
            if (m_Validate != null)
                validate = m_Validate(param);
            return validate;
        }
    }

    internal class State<TObject>
    {
        public int Id { get; private set; }
        private Action<TObject> m_OnInitialize;
        private Action<TObject> m_OnUpdate;
        private Action<TObject> m_OnEnter;
        private Action<TObject> m_OnExit;
        private Action<TObject> m_OnEarlyUpdate;

        public State(int id)
        {
            Id = id;
        }

        public void OnInitialize(TObject stateObject)
        {
            if (m_OnInitialize != null)
                m_OnInitialize(stateObject);
        }
        public void Initialize(Action<TObject> init)
        {
            m_OnInitialize = init;
        }
        public void OnEnter(TObject stateObject)
        {
            if (m_OnEnter != null)
                m_OnEnter(stateObject);
        }
        public void Enter(Action<TObject> enter)
        {
            m_OnEnter = enter;
        }
        public void OnUpdate(TObject stateObject)
        {
            if (m_OnUpdate != null)
                m_OnUpdate(stateObject);
        }
        public void Update(Action<TObject> update)
        {
            m_OnUpdate = update;
        }
        public void OnExit(TObject stateObject)
        {
            if (m_OnExit != null)
                m_OnExit(stateObject);
        }
        public void Exit(Action<TObject> exit)
        {
            m_OnExit = exit;
        }
        public void OnEarlyUpdate(TObject stateObject)
        {
            if (m_OnEarlyUpdate != null)
                m_OnEarlyUpdate(stateObject);
        }
        public void EarlyUpdate(Action<TObject> earlyUpdate)
        {
            m_OnEarlyUpdate = earlyUpdate;
        }
    }

    public class StateMachine<TObject> : IStateMachine<TObject>, IState<TObject>, ITransition<TObject> where TObject : class
    {
        private class StackState
        {
            public const byte STATE_TYPE = 1;
            public const byte TRANSITION_TYPE = 2;

            public byte Type { private set; get; }
            public object Raw { private set; get; }

            public StackState(byte type, object raw)
            {
                Type = type;
                Raw = raw;
            }
            public T RawAs<T>()
            {
                return (T)Raw;
            }
        }
        private const int ENTRY = int.MaxValue;
        private const int END = int.MinValue;
        private State<TObject> m_Current;
        private TObject m_Parameter;
        private Dictionary<int, State<TObject>> m_States;
        private Dictionary<int, List<Transition<TObject>>> m_Transitions;
        private Stack<StackState> m_StackBuilder;
        public StateMachine(TObject param)
        {
            m_States = new Dictionary<int, State<TObject>>();
            m_Transitions = new Dictionary<int, List<Transition<TObject>>>();
            m_Parameter = param;
            m_StackBuilder = new Stack<StackState>();
            m_Current = AddState(ENTRY);
            AddState(END);
        }

        private StateMachine(Dictionary<int, State<TObject>> states, Dictionary<int, List<Transition<TObject>>> transitions, TObject param)
        {
            m_States = states;
            m_Transitions = transitions;
            m_Parameter = param;

            m_Current = m_States[ENTRY];
        }

        public static StateMachine<TObject> Clone(IStateMachine<TObject> original, TObject param)
        {
            return new StateMachine<TObject>(((StateMachine<TObject>)original).m_States, ((StateMachine<TObject>)original).m_Transitions, param);
        }

        public void SetParameter<TObjecct>(TObject param) { m_Parameter = param; }
        public TObject GetParameter() { return m_Parameter; }
        public void Reset() { m_Current = m_States[ENTRY]; }
        public IStateMachine<TObject> SetDefault<TState>(TState id)
        {
            AddTransition(ENTRY, Convert.ToInt32(id), so => true, null);
            return this;
        }

        public IStateMachine<TObject> Build()
        {
            foreach (State<TObject> state in m_States.Values)
                state.OnInitialize(m_Parameter);
            return this;
        }

        private State<TObject> AddState(int id)
        {
            State<TObject> state = new State<TObject>(id);
            return AddState(state);
        }

        private State<TObject> AddState(State<TObject> state)
        {
            m_States[state.Id] = state;
            return state;
        }

        private void AddTransition(int fromId, int toId, Func<TObject, bool> valid, Action<TObject> transfer)
        {
            Transition<TObject> transition = new Transition<TObject>(fromId, toId, valid, transfer);
            AddTransition(transition);
        }

        private void AddTransition(Transition<TObject> transition)
        {
            if (!m_Transitions.ContainsKey(transition.Id))
                m_Transitions[transition.Id] = new List<Transition<TObject>>();
            m_Transitions[transition.Id].Add(transition);
        }

        public IState<TObject> State<TState>(TState id)
        {
            m_StackBuilder.Push(new StackState(StackState.STATE_TYPE, AddState(Convert.ToInt32(id))));
            return this;
        }

        IState<TObject> IState<TObject>.Initialize(Action<TObject> onInit)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Initialize(onInit);
            return this;
        }

        IState<TObject> IState<TObject>.Enter(Action<TObject> onEnter)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Enter(onEnter);
            return this;
        }

        IState<TObject> IState<TObject>.Update(Action<TObject> onUpdate)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Update(onUpdate);
            return this;
        }

        IState<TObject> IState<TObject>.EarlyUpdate(Action<TObject> onEarlyUpdate)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().EarlyUpdate(onEarlyUpdate);
            return this;
        }

        IState<TObject> IState<TObject>.Exit(Action<TObject> onExit)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Exit(onExit);
            return this;
        }

        ITransition<TObject> IState<TObject>.Transition(Func<TObject, bool> valid)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                m_StackBuilder.Push(new StackState(StackState.TRANSITION_TYPE, new Transition<TObject>(state.RawAs<State<TObject>>().Id, valid)));
            return this;
        }

        ITransition<TObject> ITransition<TObject>.Transfer(Action<TObject> onTransfer)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().SetTransfer(onTransfer);
            return this;
        }

        ITransition<TObject> ITransition<TObject>.To<TState>(TState id)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().ToId = Convert.ToInt32(id);
            return this;
        }

        ITransition<TObject> ITransition<TObject>.ToEnd()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().ToId = END;
            return this;
        }

        ITransition<TObject> ITransition<TObject>.ToEntry()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().ToId = ENTRY;
            return this;
        }

        IState<TObject> ITransition<TObject>.End()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                AddTransition(m_StackBuilder.Pop().RawAs<Transition<TObject>>());
            return this;
        }

        IStateMachine<TObject> IState<TObject>.End()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                m_StackBuilder.Pop();
            return this;
        }

        IStateMachine<TObject> IStateMachine<TObject>.Select<TState>(Func<TObject, bool> valid, TState id, TState toId, Action<TObject> transfer)
        {
            int fromStateId = Convert.ToInt32(id);
            int toStateId = Convert.ToInt32(toId);
            foreach (int stateId in m_States.Keys)
                if (stateId == (fromStateId & stateId))
                    AddTransition(stateId, toStateId, valid, transfer);
            return this;
        }

        IStateMachine<TObject> IStateMachine<TObject>.Any<TState>(Func<TObject, bool> valid, TState toId, Action<TObject> transfer)
        {
            int toStateId = Convert.ToInt32(toId);
            foreach (int stateId in m_States.Keys)
                if (stateId != toStateId)
                    AddTransition(stateId, toStateId, valid, transfer);
            return this;
        }

        void IStateMachine<TObject>.Update()
        {
            if (m_Current != null && m_Current.Id != END && m_Transitions.TryGetValue(m_Current.Id, out List<Transition<TObject>> transitions))
            {
                m_Current.OnEarlyUpdate(m_Parameter);
                foreach (Transition<TObject> transition in transitions)
                {
                    if (transition.OnValidate(m_Parameter))
                    {
                        m_Current.OnExit(m_Parameter);
                        if (m_States.ContainsKey(transition.ToId))
                        {
                            m_Current = m_States[transition.ToId];
                            transition.OnTransfer(m_Parameter);
                            m_Current.OnEnter(m_Parameter);
                        }
                        else
                        {
                            throw new Exception("Use State() to define a State.");
                        }
                        return;
                    }
                }
                m_Current.OnUpdate(m_Parameter);
            }
        }
    }
}
