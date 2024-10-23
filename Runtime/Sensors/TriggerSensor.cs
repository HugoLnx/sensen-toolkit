using UnityEngine;

namespace SensenToolkit
{
    public class TriggerSensor : SensorBase
    {
        private void OnTriggerEnter(Collider other)
        {
            OnEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(other);
        }
    }
}
