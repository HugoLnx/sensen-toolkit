using UnityEngine;

namespace SensenToolkit
{
    public class Vector3Smoother : AValueSmoother<Vector3>
    {
        public Vector3Smoother(
            float increaseDurationSecs,
            float? decreaseDurationSecs = null,
            float? decreaseThreshold = null
        ) : base(increaseDurationSecs, decreaseDurationSecs, decreaseThreshold) { }
        protected override Vector3 Minus(Vector3 a, Vector3 b) => a - b;
        protected override Vector3 Plus(Vector3 a, Vector3 b) => a + b;
        protected override Vector3 Multiply(Vector3 a, float b) => a * b;
        protected override float ToFloat(Vector3 a) => a.magnitude;
    }
}
