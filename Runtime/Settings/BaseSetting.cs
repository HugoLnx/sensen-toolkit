using UnityEngine;

namespace SensenToolkit
{
    [System.Serializable]
    public abstract class BaseSetting<T>
    {
        [SerializeField] private bool _useConstant;
        [SerializeField] private T _constant;
        public System.Func<T> Getter { private get; set; }
        public T Value => _useConstant || Getter == null ? _constant : Getter();
    }
}
