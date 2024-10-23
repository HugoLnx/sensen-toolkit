using System;
using UnityEngine;

namespace SensenToolkit
{
    public class SensorTarget : MonoBehaviour
    {
        public event Action<SensorTarget> OnDestroyed;
        public event Action<SensorTarget> OnDisabled;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke(this);
        }
    }
}
