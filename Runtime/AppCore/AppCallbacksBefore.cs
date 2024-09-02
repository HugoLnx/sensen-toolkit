using UnityEngine;

namespace SensenToolkit
{
    [DefaultExecutionOrder(AppCore.BEFORE_ORDER)]
    public class AppCallbacksBefore : MonoBehaviour
    {
        public event System.Action BeforeAwake;
        public event System.Action BeforeStart;
        public event System.Action BeforeEnable;
        public event System.Action BeforeDisable;
        public event System.Action BeforeDestroy;
        public event System.Action BeforeQuit;

        private void Awake() => BeforeAwake?.Invoke();
        private void Start() => BeforeStart?.Invoke();
        private void OnEnable() => BeforeEnable?.Invoke();
        private void OnDisable() => BeforeDisable?.Invoke();
        private void OnDestroy() => BeforeDestroy?.Invoke();
        private void OnApplicationQuit() => BeforeQuit?.Invoke();
    }
}
