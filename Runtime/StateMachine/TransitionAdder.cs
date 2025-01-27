using System.Collections.Generic;

namespace SensenToolkit
{
    public class TransitionAdder<TState, TStateId, TMessage>
    where TState : AState<TStateId>
    where TStateId : struct, System.Enum
    where TMessage : struct, System.Enum
    {
        private AStateMachine<TState, TStateId, TMessage> _fsm;
        private List<TStateId> _from;

        public TransitionAdder(AStateMachine<TState, TStateId, TMessage> fsm, TStateId[] from)
        {
            _fsm = fsm;
            _from = new List<TStateId>(from);
        }

        public TransitionAdder<TState, TStateId, TMessage> To(TMessage message, TStateId to)
        {
            foreach (TStateId state in _from)
            {
                _fsm.AddTransition(state, message, to);
            }
            return this;
        }

        public TransitionAdder<TState, TStateId, TMessage> Chain(TMessage message, TStateId to)
        {
            To(message, to);
            _from.Clear();
            _from.Add(to);
            return this;
        }
    }
}
