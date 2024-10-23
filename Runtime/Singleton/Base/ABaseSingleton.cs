using System;
using UnityEngine;

namespace SensenToolkit.Internal
{
    public abstract class ABaseSingleton<T> : ASuperBaseSingleton<T>
    where T : ABaseSingleton<T>
    {
        private static string s_singletonTypeName;
        private static string SingletonTypeName => s_singletonTypeName ??= IsPermanent ? "Permanent" : "Transient";
        protected sealed override string DescriptiveKey => $"{typeof(T).Name}|{SingletonTypeName}|{name}";

        private static T s_instance;
        public static bool HasInstance => s_instance != null;
        protected sealed override bool IsAlreadyInstanced => HasInstance;
        public override bool IsActualSingleton => s_instance == this;
        public override bool IsExcessSingleton => s_instance != null && s_instance != this;
        public static T Instance => GetOrSetInstance();

        private static Logx s_logger;
        private static Logx Logger => s_logger ??= Logx.GetLogger(LOGGER_ID);

        public static void AddAssignListener(Action<T> setSingleton)
        {
            OnSetSingleton += setSingleton;
            if (HasInstance) setSingleton(s_instance);
        }

        public static void RemoveAssignListener(Action<T> setSingleton)
        {
            OnSetSingleton -= setSingleton;
        }


        private static T GetOrSetInstance()
        {
            if (HasInstance) return s_instance;

            if (IsTransient && AppCore.IsSceneUnloading)
            {
                // Scene is unloading, should not create new instance
                return null;
            }

            if (AppCore.IsQuitting)
            {
                // Application is quitting, should not create new instance
                return null;
            }

            if (TrySetAndInitializeInstance(FindObjectOfType<T>()))
            {

                Assertx.IsNotNull(s_instance);
                Logger.Info($"[{s_instance.DescriptiveKey}] Binded found instance");
                return s_instance;
            }

            var obj = new GameObject();
            obj.SetActive(false);
            if (TrySetAndInitializeInstance(obj.AddComponent<T>()))
            {
                obj.name = $"{typeof(T).Name}:AutoCreated:{SingletonTypeName}";
                obj.SetActive(true);

                Assertx.IsNotNull(s_instance);
                Logger.Info($"[{obj.name}] Binded created instance");
                return s_instance;
            }

            Destroy(obj);
            throw new Exception($"[{typeof(T).Name}] Failed to setup instance");
        }

        protected sealed override bool ToSingletonInstance() => TrySetAndInitializeInstance(this as T);
        private static bool TrySetAndInitializeInstance(T instance)
        {
            if (instance == null) return false;
            if (HasInstance) throw new Exception($"[{instance.DescriptiveKey}] Instance already set");

            s_instance = instance;
            instance.InitializeAsSingleton();
            return true;
        }

        protected sealed override void ResetStatics()
        {
            base.ResetStatics();
            s_instance = null;
        }

        protected sealed override void OnDestroySingletonInternal()
        {
            s_instance = null;
        }
    }
}
