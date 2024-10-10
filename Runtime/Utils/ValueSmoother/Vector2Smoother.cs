using UnityEngine;

namespace SensenToolkit
{
    public class Vector2Smoother : AValueSmoother<Vector2>
    {
        public Vector2Smoother(int bufferSize) : base(bufferSize, Vector2.zero) { }

        protected override Vector2 Divide(Vector2 a, int b) => a / b;
        protected override Vector2 Minus(Vector2 a, Vector2 b) => a - b;
        protected override Vector2 Plus(Vector2 a, Vector2 b) => a + b;
    }
}
