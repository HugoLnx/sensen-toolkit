namespace SensenToolkit
{
    public class TransitionAdder<TState, TStateId, TMessage>
    where TState : AState<TStateId>
    where TStateId : struct, System.Enum
    where TMessage : struct, System.Enum
    {
        private AStateMachine<TState, TStateId, TMessage> _fsm;
        private TStateId[] _from;

        public TransitionAdder(AStateMachine<TState, TStateId, TMessage> fsm, TStateId[] from)
        {
            _fsm = fsm;
            _from = from;
        }

        public TransitionAdder<TState, TStateId, TMessage> To(TMessage message, TStateId to)
        {
            foreach (TStateId state in _from)
            {
                _fsm.AddTransition(state, message, to);
            }
            return this;
        }
    }
}
