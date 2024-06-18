using System;
using System.Collections.Generic;

namespace Synchronize.Game.Lockstep.FSM
{
    public class FsmEventArgs
    {
        public string EventType;
        public object EventParameter;

        public FsmEventArgs(string eventType, object eventParameter)
        {
            EventType = eventType;
            EventParameter = eventParameter;
        }

        public T ParameterAs<T>()
        {
            return (T)EventParameter;
        }

        public override string ToString()
        {
            return $"EventType:{EventType} Parameter:{EventParameter.ToString()}";
        }

        public static FsmEventArgs Take(List<FsmEventArgs> evts, string evtType, bool needRemoveEvent = true)
        {
            for (int i = evts.Count - 1; i > -1; i--)
            {
                if (evts[i].EventType == evtType)
                {
                    var evt = evts[i];
                    if (needRemoveEvent)
                        evts.RemoveAt(i);
                    return evt;
                }
            }
            return null;
        }

        public static bool Poll(List<FsmEventArgs> evts, string evtType, bool needRemoveEvent = true)
        {
            for (int i = evts.Count - 1; i > -1; i--)
            {
                if (evts[i].EventType == evtType)
                {
                    if (needRemoveEvent)
                        evts.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
    public interface IStateMachine<TObject>
    {
        void SetParameter(TObject param);
        TObject GetParameter();
        void Reset();
        IStateDeclarable<TObject> State<TState>(TState id);
        IStateMachine<TObject> SetDefault<TState>(TState id);
        void Update();
        IStateMachine<TObject> Build();
        IStateMachine<TObject> Any<TState>(Func<TObject, bool> valid, TState toId, Action<TObject> transfer);
        IStateMachine<TObject> Select<TState>(Func<TObject, bool> valid, TState id, TState toId, Action<TObject> transfer);
    }

    public interface IStateDeclarable<TObject>
    {
        IStateDeclarable<TObject> Initialize(Action<TObject> onInit);
        IStateDeclarable<TObject> Enter(Action<TObject> onEnter);
        IStateDeclarable<TObject> EarlyUpdate(Action<TObject> onEarlyUpdate);
        IStateDeclarable<TObject> Update(Action<TObject> onUpdate);
        IStateDeclarable<TObject> Exit(Action<TObject> onExit);
        IStateMachine<TObject> End();
        ITransitionDeclarable<TObject> Transition(Func<TObject, bool> valid);
    }

    public interface ITransitionDeclarable<TObject>
    {
        ITransitionDeclarable<TObject> Transfer(Action<TObject> onTransfer);
        ITransitionDeclarable<TObject> To<TState>(TState id);
        ITransitionDeclarable<TObject> Return();
        ITransitionDeclarable<TObject> ToEnd();
        ITransitionDeclarable<TObject> ToEntry();
        IStateDeclarable<TObject> End();
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

    public class State<TObject>
    {
        public int PreviousId = -1;
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
    public class StateMachineConst
    {
        public const int ENTRY = int.MaxValue;
        public const int END = int.MinValue;
    }
    public class StackState
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
    public sealed class StateMachine<TObject> : IStateMachine<TObject>, IStateDeclarable<TObject>, ITransitionDeclarable<TObject> where TObject : class
    {
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
            m_Current = AddState(StateMachineConst.ENTRY);
            AddState(StateMachineConst.END);
        }

        private StateMachine(Dictionary<int, State<TObject>> states, Dictionary<int, List<Transition<TObject>>> transitions, TObject param)
        {
            m_States = states;
            m_Transitions = transitions;
            m_Parameter = param;

            m_Current = m_States[StateMachineConst.ENTRY];
        }

        public static StateMachine<TObject> Clone(IStateMachine<TObject> original, TObject param)
        {
            return new StateMachine<TObject>(((StateMachine<TObject>)original).m_States, ((StateMachine<TObject>)original).m_Transitions, param);
        }
        public void SetParameter(TObject param) { m_Parameter = param; }
        public TObject GetParameter() { return m_Parameter; }
        public void Reset() { m_Current = m_States[StateMachineConst.ENTRY]; }
        public IStateMachine<TObject> SetDefault<TState>(TState id)
        {
            AddTransition(StateMachineConst.ENTRY, Convert.ToInt32(id), so => true, null);
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

        public IStateDeclarable<TObject> State<TState>(TState id)
        {
            m_StackBuilder.Push(new StackState(StackState.STATE_TYPE, AddState(Convert.ToInt32(id))));
            return this;
        }

        IStateDeclarable<TObject> IStateDeclarable<TObject>.Initialize(Action<TObject> onInit)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Initialize(onInit);
            return this;
        }

        IStateDeclarable<TObject> IStateDeclarable<TObject>.Enter(Action<TObject> onEnter)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Enter(onEnter);
            return this;
        }

        IStateDeclarable<TObject> IStateDeclarable<TObject>.Update(Action<TObject> onUpdate)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Update(onUpdate);
            return this;
        }

        IStateDeclarable<TObject> IStateDeclarable<TObject>.EarlyUpdate(Action<TObject> onEarlyUpdate)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().EarlyUpdate(onEarlyUpdate);
            return this;
        }

        IStateDeclarable<TObject> IStateDeclarable<TObject>.Exit(Action<TObject> onExit)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                state.RawAs<State<TObject>>().Exit(onExit);
            return this;
        }

        ITransitionDeclarable<TObject> IStateDeclarable<TObject>.Transition(Func<TObject, bool> valid)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.STATE_TYPE)
                m_StackBuilder.Push(new StackState(StackState.TRANSITION_TYPE, new Transition<TObject>(state.RawAs<State<TObject>>().Id, valid)));
            return this;
        }

        ITransitionDeclarable<TObject> ITransitionDeclarable<TObject>.Transfer(Action<TObject> onTransfer)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().SetTransfer(onTransfer);
            return this;
        }

        ITransitionDeclarable<TObject> ITransitionDeclarable<TObject>.To<TState>(TState id)
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().ToId = Convert.ToInt32(id);
            return this;
        }
        ITransitionDeclarable<TObject> ITransitionDeclarable<TObject>.Return()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().ToId = -1;
            return this;
        }
        ITransitionDeclarable<TObject> ITransitionDeclarable<TObject>.ToEnd()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().ToId = StateMachineConst.END;
            return this;
        }

        ITransitionDeclarable<TObject> ITransitionDeclarable<TObject>.ToEntry()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                state.RawAs<Transition<TObject>>().ToId = StateMachineConst.ENTRY;
            return this;
        }

        IStateDeclarable<TObject> ITransitionDeclarable<TObject>.End()
        {
            StackState state = m_StackBuilder.Peek();
            if (state.Type == StackState.TRANSITION_TYPE)
                AddTransition(m_StackBuilder.Pop().RawAs<Transition<TObject>>());
            return this;
        }

        IStateMachine<TObject> IStateDeclarable<TObject>.End()
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
            if (m_Current != null && m_Current.Id != StateMachineConst.END && m_Transitions.TryGetValue(m_Current.Id, out List<Transition<TObject>> transitions))
            {
                m_Current.OnEarlyUpdate(m_Parameter);
                foreach (Transition<TObject> transition in transitions)
                {
                    if (transition.OnValidate(m_Parameter))
                    {
                        m_Current.OnExit(m_Parameter);

                        // Return type: If transition's toId == -1, it's toId will redirect to the previous state into the existing state. 
                        int toId = transition.ToId == -1 ? m_Current.PreviousId : transition.ToId;

                        if (m_States.TryGetValue(toId, out State<TObject> next))
                        {
                            int previousId = m_Current.Id;
                            m_Current = next;
                            m_Current.PreviousId = previousId;
                            transition.OnTransfer(m_Parameter);
                            m_Current.OnEnter(m_Parameter);
                        }
                        else
                        {
                            throw new Exception("Use State() to define a State." + toId);
                        }
                        return;
                    }
                }
                m_Current.OnUpdate(m_Parameter);

            }
        }
    }
}
