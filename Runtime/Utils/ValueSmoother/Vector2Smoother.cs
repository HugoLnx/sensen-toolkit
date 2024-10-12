using UnityEngine;

namespace SensenToolkit
{
    public class Vector2Smoother : AValueSmoother<Vector2>
    {
        public Vector2Smoother(
            float increaseDurationSecs,
            float? decreaseDurationSecs = null,
            float? decreaseThreshold = null
        ) : base(increaseDurationSecs, decreaseDurationSecs, decreaseThreshold) { }
        protected override Vector2 Minus(Vector2 a, Vector2 b) => a - b;
        protected override Vector2 Plus(Vector2 a, Vector2 b) => a + b;
        protected override Vector2 Multiply(Vector2 a, float b) => a * b;
        protected override float ToFloat(Vector2 a) => a.magnitude;
    }
}
