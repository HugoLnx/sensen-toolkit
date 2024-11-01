using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace SensenToolkit
{
    [System.Serializable]
    public abstract class ADebuggerLoop<T>
    where T : MonoBehaviour
    {
        protected T Target;

        protected abstract IEnumerator Loop();

        public ADebuggerLoop(T t)
        {
            this.Target = t;
        }

        public ADebuggerLoop<T> StartLoop()
        {
            Target.StartCoroutine(Loop());
            return this;
        }
    }
}
