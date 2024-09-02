using UnityEngine;

namespace SensenToolkit
{
    [DefaultExecutionOrder(AppCore.AFTER_ORDER)]
    public class AppCallbacksAfter : MonoBehaviour
    {
        public event System.Action AfterAwake;
        public event System.Action AfterStart;
        public event System.Action AfterEnable;
        public event System.Action AfterDisable;
        public event System.Action AfterDestroy;
        public event System.Action AfterQuit;

        private void Awake() => AfterAwake?.Invoke();
        private void Start() => AfterStart?.Invoke();
        private void OnEnable() => AfterEnable?.Invoke();
        private void OnDisable() => AfterDisable?.Invoke();
        private void OnDestroy() => AfterDestroy?.Invoke();
        private void OnApplicationQuit() => AfterQuit?.Invoke();
    }
}
