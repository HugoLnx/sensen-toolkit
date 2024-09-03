using UnityEngine;

namespace SensenToolkit.Internal
{
    public class TransientSingletonAttribute : System.Attribute { }

    [DefaultExecutionOrder(-1)]
    public abstract class ASuperBaseSingleton<T> : MonoBehaviour
    where T : ASuperBaseSingleton<T>
    {
        private static bool? s_isTransient;
        protected static bool IsTransient => s_isTransient ??= typeof(T).IsDefined(typeof(TransientSingletonAttribute), true);
        protected static bool IsPermanent => !IsTransient;
        protected abstract bool IsAlreadyInstanced { get; }
        public abstract bool IsActualSingleton { get; }
        public abstract bool IsExcessSingleton { get; }
        protected abstract string DescriptiveKey { get; }
        protected abstract bool ToSingletonInstance();
        protected abstract void OnDestroySingletonInternal();

        protected const string LOGGER_ID = "Singleton";
        private static Logx s_logger;
        private static Logx Logger => s_logger ??= Logx.GetLogger(LOGGER_ID);

        public static event System.Action<T> OnSetSingleton;

        protected void InitializeAsSingleton()
        {

            if (IsPermanent)
            {
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            OnSetSingleton?.Invoke(this as T);
        }


        // Needed callbacks to avoid calling them on the excess instances.
        protected virtual void AwakeSingleton() { }
        protected virtual void OnDestroySingleton() { }
        protected virtual void OnDisableSingleton() { }

        protected virtual void AwakeExcess() { }
        protected virtual void OnDestroyExcess() { }
        protected virtual void OnDisableExcess() { }

        protected virtual void AwakeAny() { }
        protected virtual void OnDestroyAny() { }
        protected virtual void OnDisableAny() { }

        protected object ResetStaticsId => _resetStaticsId ??= (typeof(T), "reset-statics");
        private object _resetStaticsId;

        protected void Awake()
        {
            AppCore.RunOnlyOnce(ResetStaticsId, ResetStatics);
            if (!IsAlreadyInstanced && (this as T).ToSingletonInstance())
            {
                Logger.Info($"[{DescriptiveKey}] Binded awake instance");
            }

            AwakeAny();
            if (IsActualSingleton) AwakeSingleton();
            else
            {
                Destroy(gameObject);
                AwakeExcess();
            }
        }

        protected void OnDestroy()
        {
            OnDestroyAny();
            if (IsActualSingleton)
            {
                OnDestroySingleton();
                OnDestroySingletonInternal();
            }
            else OnDestroyExcess();
        }

        protected void OnDisable()
        {
            OnDisableAny();
            if (IsActualSingleton) OnDisableSingleton();
            else OnDisableExcess();
        }

        protected virtual void ResetStatics()
        {
            Logger.Info($"[{DescriptiveKey}] ResetStatics");
            OnSetSingleton = null;
            s_isTransient = null;
        }
    }
}
