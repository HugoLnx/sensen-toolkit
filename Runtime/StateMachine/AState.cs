using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit
{
    public abstract class AState<TStateId> : MonoBehaviour
    where TStateId : struct, System.Enum
    {
        public abstract TStateId Id { get; }
        public virtual HashSet<TStateId> GroupIds => null;
        private bool _isActive = false;
        protected virtual void OnStateEnter() { }
        protected virtual void OnStateExit() { }
        internal void OnStateEnterInternal()
        {
            this.enabled = true;
            _isActive = true;
            OnStateEnter();
        }

        internal void OnStateExitInternal()
        {
            OnStateExit();
            _isActive = false;
            this.enabled = false;
        }

        protected void OnDisable()
        {
            if (_isActive)
            {
                OnStateExitInternal();
            }
        }
    }
}
