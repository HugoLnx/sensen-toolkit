using UnityEngine;

namespace SensenToolkit
{
    public abstract class AState<TStateId> : MonoBehaviour
    where TStateId : struct, System.Enum
    {
        public abstract TStateId Id { get; }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
    }
}
