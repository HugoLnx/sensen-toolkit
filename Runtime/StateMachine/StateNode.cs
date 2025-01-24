using System.Collections.Generic;

namespace SensenToolkit
{
    public class StateNode<TState, TStateId, TMessage>
    where TState : AState<TStateId>
    where TStateId : struct, System.Enum
    where TMessage : struct, System.Enum
    {
        public TState State { get; private set; }
        public Dictionary<TMessage, StateNode<TState, TStateId, TMessage>> NextStates { get; }
            = new();

        public StateNode(TState state)
        {
            State = state;
        }

        public StateNode<TState, TStateId, TMessage> AddTransition(
            TMessage message, StateNode<TState, TStateId, TMessage> nextState)
        {
            NextStates.Add(message, nextState);
            return this;
        }
    }
}
