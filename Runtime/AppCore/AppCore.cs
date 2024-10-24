using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EasyButtons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SensenToolkit
{
    [DefaultExecutionOrder(AppCore.CORE_ORDER)]
    public class AppCore : MonoBehaviour
    {
        private const int CORE_ORDER = -999999;
        public const int BEFORE_ORDER = -999998;
        public const int AFTER_ORDER = 999999;
        private static AppCore s_instance;
        private static readonly HashSet<int> s_lifetimeActions = new();
        private static readonly HashSet<int> s_sceneActions = new();

        public static event Action<Scene> OnSceneLoadStart;
        public static event Action<Scene> OnSceneLoadEnd;
        public static event Action<Scene> OnSceneUnloadStart;
        public static event Action<Scene> OnSceneUnloadEnd;
        public static event Action OnQuittingStart;

        public static bool IsSceneLoading { get; private set; }
        public static bool IsSceneUnloading { get; private set; }
        public static bool IsQuitting { get; private set; }

        [SerializeField] private Transform _callbacksContainer;

        public static void RunOnlyOnce(object id, Action action)
        {
            if (action == null) return;
            int hash = id.GetHashCode();
            if (s_lifetimeActions.Contains(hash)) return;
            s_lifetimeActions.Add(hash);
            action();
        }

        /// <summary>
        /// Run the action only once in the game lifetime. Don't pass a instance method directly, use a lambda or a static method.
        /// </summary>
        public static void RunOnlyOnce(System.Action action)
        {
            RunOnlyOnce(action, action);
        }

        public static void RunOncePerScene(object id, System.Action action)
        {
            if (action == null) return;
            int hash = id.GetHashCode();
            if (s_sceneActions.Contains(hash)) return;
            s_sceneActions.Add(hash);
            action();
        }

        public static void RunOncePerScene(System.Action action)
        {
            RunOncePerScene(action, action);
        }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // private static void BeforeSceneLoad()
        // {
        //     Debug.Log("RuntimeInitialize: BeforeSceneLoad");
        // }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            s_lifetimeActions.Clear();
            s_sceneActions.Clear();
            OnSceneLoadStart = null;
            OnSceneLoadEnd = null;
            OnSceneUnloadStart = null;
            OnSceneUnloadEnd = null;
            IsSceneLoading = false;
            IsSceneUnloading = false;
            IsQuitting = false;

            OnSceneLoadStart += _ =>
            {
                s_sceneActions.Clear();
            };

            // This is needed to avoid the event being added multiple times when
            // domain reload is disabled on Editor
            Application.quitting -= TryCallQuittingStart;
            Application.quitting += TryCallQuittingStart;
            IsQuitting = false;
        }

        private void Awake()
        {
            if (_callbacksContainer == null)
            {
                Debug.LogError("[AppCore] _callbacksContainer can't be null");
                return;
            }
            if (_callbacksContainer.gameObject == this.gameObject)
            {
                Debug.LogError("[AppCore] _callbacksContainer can't be in the same GameObject as AppCore");
                return;
            }
            AppCallbacksAfter after = _callbacksContainer.GetComponentInChildren<AppCallbacksAfter>();
            if (after == null)
            {
                Debug.LogError("[AppCore] AppCallbacksAfter not found in _callbacksContainer");
                return;
            }
            AppCallbacksBefore before = _callbacksContainer.GetComponentInChildren<AppCallbacksBefore>();
            if (before == null)
            {
                Debug.LogError("[AppCore] AppCallbacksBefore not found in _callbacksContainer");
                return;
            }
            if (s_instance == null) s_instance = this;
            TryCallOnSceneLoadStart();

            after.AfterDisable += () => TryCallOnSceneUnloadStart();
            after.AfterDestroy += () =>
            {
                SceneManager.sceneUnloaded -= CallOnSceneUnloadEnd;
                SceneManager.sceneUnloaded += CallOnSceneUnloadEnd;
            };
            after.AfterQuit += () => TryCallQuittingStart();
            before.BeforeStart += () => TryCallOnSceneLoadEnd();
            before.BeforeDisable += () => TryCallOnSceneUnloadStart();
            before.BeforeQuit += () => TryCallQuittingStart();

            _callbacksContainer.SetParent(null);
            _callbacksContainer.SetAsFirstSibling();

            if (s_instance == this)
            {
                transform.SetParent(null);
                transform.SetAsFirstSibling();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private static void TryCallQuittingStart()
        {
            if (OnQuittingStart != null && !IsQuitting)
            {
                IsQuitting = true;
                OnQuittingStart.Invoke();
                TryCallOnSceneUnloadStart();
            }
        }

        private static void TryCallOnSceneLoadStart()
        {
            if (IsSceneLoading) return;
            IsSceneLoading = true;
            if (OnSceneLoadStart != null)
            {
                OnSceneLoadStart.Invoke(SceneManager.GetActiveScene());
            }
        }

        private static void TryCallOnSceneLoadEnd()
        {
            if (OnSceneLoadEnd != null)
            {
                OnSceneLoadEnd.Invoke(SceneManager.GetActiveScene());
            }
            IsSceneLoading = false;
        }

        private static void TryCallOnSceneUnloadStart()
        {
            if (IsSceneUnloading) return;
            IsSceneUnloading = true;
            if (OnSceneUnloadStart != null)
            {
                OnSceneUnloadStart.Invoke(SceneManager.GetActiveScene());
            }
        }

        private static void CallOnSceneUnloadEnd(Scene scene)
        {
            OnSceneUnloadEnd?.Invoke(scene);
            IsSceneUnloading = false;
        }

        [Button]
        private void Setup()
        {
            AppCallbacksBefore before = GetComponentInChildren<AppCallbacksBefore>();
            AppCallbacksAfter after = GetComponentInChildren<AppCallbacksAfter>();
            Transform container = _callbacksContainer;
            if (container == null && before != null) container = before.transform;
            if (container == null && after != null) container = after.transform;
            if (container == null) container = new GameObject("AppCore:Callbacks").transform;
            container.SetParent(this.transform);

            if (before == null || before.transform != container)
            {
                before = container.gameObject.AddComponent<AppCallbacksBefore>();
            }

            if (after == null || after.transform != container)
            {
                after = container.gameObject.AddComponent<AppCallbacksAfter>();
            }
            _callbacksContainer = container;

            foreach (AppCallbacksAfter otherAfter in GetComponentsInChildren<AppCallbacksAfter>())
            {
                if (otherAfter != after)
                {
                    DestroyImmediate(otherAfter);
                }
            }

            foreach (AppCallbacksBefore otherBefore in GetComponentsInChildren<AppCallbacksBefore>())
            {
                if (otherBefore != before)
                {
                    DestroyImmediate(otherBefore);
                }
            }
        }
    }
}
