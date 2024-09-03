using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SensenToolkit.Internal
{
    public abstract class ANamedBaseSingleton<T> : ASuperBaseSingleton<T>
    where T : ANamedBaseSingleton<T>
    {
        private static readonly Regex s_singletonNameRegex = new(@"^[^:]+", RegexOptions.Compiled);
        [SerializeField] protected string _singletonName;
        private string SingletonName => _singletonName = String.IsNullOrEmpty(_singletonName)
            ? s_singletonNameRegex.Match(gameObject.name).Value
            : _singletonName;

        public static T NamedInstance(string singletonName) => GetOrSetInstanceByName(singletonName);
        public override bool IsActualSingleton => s_instancesMap.TryGetValue(Id, out T instance) && instance == this;
        public override bool IsExcessSingleton => s_instancesMap.TryGetValue(Id, out T instance) && instance != this;

        private int? _id;
        private int Id => _id ??= SingletonNameToId(SingletonName);

        private static string s_singletonTypeName;
        private static string SingletonTypeName => s_singletonTypeName ??= IsPermanent ? "NamedPermanent" : "NamedTransient";

        private static readonly Dictionary<int, T> s_instancesMap = new();
        private static readonly HashSet<int> s_ids = new();
        protected sealed override bool IsAlreadyInstanced => s_ids.Contains(Id);

        protected sealed override string DescriptiveKey => $"{typeof(T).Name}|{SingletonTypeName}|{Id}|{SingletonName}|{name}";

        private static Logx s_logger;
        private static Logx Logger => s_logger ??= Logx.GetLogger(LOGGER_ID);

        private static T GetOrSetInstanceByName(string singletonName)
        {
            Assertx.IsNotNull(singletonName, "Singleton name is null");
            int id = SingletonNameToId(singletonName);
            if (s_instancesMap.TryGetValue(id, out T instance)) return instance;

            instance = FindObjectsOfType<T>().FirstOrDefault(i => i.Id == id);
            if (TrySetAndInitializeInstance(instance))
            {
                Logger.Info($"[{instance.DescriptiveKey}] Binded found instance");
                return instance;
            }

            var obj = new GameObject($"{singletonName}:{typeof(T).Name}:AutoCreated:{SingletonTypeName}");
            obj.SetActive(false);
            instance = obj.AddComponent<T>();
            if (TrySetAndInitializeInstance(instance))
            {
                Logger.Info($"[{obj.name}] Binded created instance");
                obj.SetActive(true);
                return instance;
            }

            Destroy(obj);
            throw new Exception($"[{typeof(T).Name}] Failed to setup instance");
        }

        protected sealed override bool ToSingletonInstance() => TrySetAndInitializeInstance(this as T);
        private static bool TrySetAndInitializeInstance(T instance)
        {
            if (instance == null) return false;

            if (!s_instancesMap.TryAdd(instance.Id, instance))
            {
                throw new Exception($"[{instance.DescriptiveKey}] Instance already set");
            }
            s_ids.Add(instance.Id);

            instance.InitializeAsSingleton();
            return true;
        }

        protected sealed override void ResetStatics()
        {
            base.ResetStatics();
            s_instancesMap.Clear();
            s_ids.Clear();
        }

        private static int SingletonNameToId(string singName)
        {
            return singName.GetHashCode();
        }

        protected sealed override void OnDestroySingletonInternal()
        {
            s_instancesMap.Remove(Id);
            s_ids.Remove(Id);
        }
    }
}
